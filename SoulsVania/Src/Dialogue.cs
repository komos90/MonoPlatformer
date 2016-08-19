using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    public class Dialogue {
        private const int totalLines = 4;
        public delegate void ResponseFunc(World world);
        private Color borderColour;
        private Color backColour;
        private Color stringColour;
        private Rectangle posDims;
        private List<string> lines;
        private int optionPointer = 0;
        private bool hasProfile;
        private Texture2D profile;
        private bool isQuestion;
        private int currentLine;
        private int selectedOptionPtr;
        public bool DialogueDone;
        private Dictionary<int, ResponseFunc> responseFunctions;
        private int fullLinesDisplayed;
        private int charactersDisplayed;
        private float charWaitTime;
        private float optionWaitTime;
        private Timer charWaitTimer;
        private int continueYOffset;
        private int linesLeft;
        //private static void noResponse(World world) {}

        public Dialogue(string text, Texture2D profile=null, Dictionary<int, ResponseFunc> responses = null, Color ? backColour=null, Color? borderColour=null, Color? stringColour=null, int x= 64, int y = 320, int width=672, int height=128) {
            this.backColour = backColour ?? Color.DarkSlateGray;
            this.borderColour = borderColour ?? Color.White;
            this.stringColour = stringColour ?? Color.White;
            posDims = new Rectangle(x, y, width, height);
            this.profile = profile;
            if (profile != null) {
                hasProfile = true;
            }
            currentLine = 0;
            lines = new List<string>();
            selectedOptionPtr = 3;
            isQuestion = true;
            DialogueDone = false;
            responseFunctions = responses == null? new Dictionary<int, ResponseFunc>() : responses;
            charWaitTime = 0.1f;
            optionWaitTime = 0.4f;
            charWaitTimer = new Timer();

            // Prepare Text
            int lineStartPtr = 0;
            for (int i = 0; i < text.Length; i++) {
                if (text[i] == '<') {
                    switch(text[i+1]) {
                        case 'b':
                            lines.Add(text.Substring(lineStartPtr, i - lineStartPtr));
                            lineStartPtr = i + 3;
                            break;
                    }
                }
            }
            currentLine -= totalLines;
            turnPage();
            linesLeft = lines.Count;
        }

        private void turnPage() {
            currentLine += totalLines;
            isQuestion = false;
            if (currentLine >= lines.Count) {
                DialogueDone = true;
            }
            for (int i = currentLine; i < currentLine + totalLines; i++) {
                if (i >= lines.Count) {
                    break;
                }
                var line = lines[i];
                if (line[0] == '<' && (char.IsDigit(line[1]))) {
                    if (!isQuestion) {
                        isQuestion = true;
                        optionPointer = i - currentLine;
                    }
                }
            }
            linesLeft -= totalLines;
            fullLinesDisplayed = 0;
            charactersDisplayed = 0;
        }

        private int relativeLineToOptionId(int relLine) {
            if (currentLine + relLine >= lines.Count || currentLine + relLine < 0) return -1;
            var line = lines[currentLine + relLine];
            int s = 0, e = 0;
            for (int i = 0; i < line.Length; i++) {
                if (line[i] == '<') { s = i + 1; }
                else if (line[i] == '>') { e = i; }
            }
            if (s == e) return -1;
            return Convert.ToInt32(line.Substring(s, e - s));
        }

        public void Input(World world) {
            if (KeyBind.WasCommandPressed(KeyBind.Command.MenuConfirm)) {
                if (isQuestion) {
                    ResponseFunc response;
                    if(responseFunctions.TryGetValue(relativeLineToOptionId(optionPointer), out response)) {
                        response(world);
                    }
                    turnPage();
                } else {
                    turnPage();
                }
            }
            /*foreach (var line in lines) {
                if (line[0] == '@' && (line[1] == '^' || line[1] == '.' || line[1] == '_')) {
                    isQuestion = true;
                }
            }*/

            if (isQuestion) {
                if (KeyBind.WasCommandPressed(KeyBind.Command.MenuDown)) {
                    //NOTE: replace 4s with constant
                    optionPointer = (optionPointer + 1) % totalLines;
                    while (relativeLineToOptionId(optionPointer) == -1) {
                        optionPointer = (optionPointer + 1) % totalLines;
                    }
                } else if (KeyBind.WasCommandPressed(KeyBind.Command.MenuUp)) {
                    //NOTE: replace 4s with constant
                    optionPointer = (optionPointer - 1);
                    if (optionPointer < 0) { optionPointer += totalLines; }
                    while (relativeLineToOptionId(optionPointer) == -1) {
                        optionPointer = (optionPointer - 1);
                        if (optionPointer < 0) { optionPointer += totalLines; }
                    }
                }
            }
        }

        public void Update(GameTime gameTime) {
            continueYOffset = (int)(2 * Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2.0 * Math.PI));
            bool done = false;
            int actualLines = Math.Min(totalLines, linesLeft);
            if (fullLinesDisplayed != actualLines && charWaitTimer.IsDone(gameTime)) {
                // HACK:
                if (charactersDisplayed == 0 && lines[currentLine + fullLinesDisplayed][0] == '<') {
                    Sfx.TextNoise.Play();
                    fullLinesDisplayed++;
                    done = true;
                }
                if (currentLine + fullLinesDisplayed < lines.Count && lines[currentLine + fullLinesDisplayed][0] == '<') {
                    charWaitTimer.Wait(gameTime, optionWaitTime);
                } else {
                    charWaitTimer.Wait(gameTime, charWaitTime);
                }
                if (done) { return; }
                charactersDisplayed++;
                Sfx.TextNoise.Play();
                if (charactersDisplayed >= lines[currentLine + fullLinesDisplayed].Length) {
                    charactersDisplayed = 0;
                    fullLinesDisplayed++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            int actualLines = Math.Min(totalLines, lines.Count);
            Rectangle fillRect = new Rectangle(posDims.X + 2, posDims.Y + 2, posDims.Width - 4, posDims.Height - 4);
            int textXOffset = 24 + (hasProfile ? 128 : 24);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.Draw(Images.DebugRect, fillRect, backColour);
            if (hasProfile) {
                spriteBatch.Draw(profile, new Vector2(posDims.X + 16, posDims.Y + 16), Color.White);
            }
            for (int i = currentLine; i < currentLine + totalLines; i++) {
                if (i >= lines.Count) break;
                var line = lines[i];
                var extraOffset = 0;
                if (lines[i][0] == '<') {
                    int start = 0;
                    for (int j = 0; j < lines[i].Length; j++) {
                        if (lines[i][j] == '>') {
                            start = j + 1;
                        }
                    }
                    line = lines[i].Substring(start);
                    extraOffset = 48;
                }
                if (i - currentLine > fullLinesDisplayed) {
                    line = "";
                } else if (i - currentLine == fullLinesDisplayed) {
                    line = line.Substring(0, Math.Min(charactersDisplayed, line.Length));
                }
                spriteBatch.DrawString(Fonts.OpenSans, line, new Vector2(posDims.X + textXOffset + extraOffset, posDims.Y + 24 + (i - currentLine) * 24), stringColour);
            }
            if (fullLinesDisplayed == actualLines && isQuestion) {
                spriteBatch.Draw(Images.GetImage("dialogue_cursor"), new Vector2(posDims.X + textXOffset, posDims.Y + 24 + (optionPointer) * 24), stringColour);
            } else if (fullLinesDisplayed == totalLines) {
                spriteBatch.Draw(Images.GetImage("dialogue_continue"), new Vector2(posDims.X + posDims.Width - 32, posDims.Y + posDims.Height - 32 + continueYOffset), stringColour);
            }
            int tW = posDims.Width / 16;
            int tH = posDims.Height / 16;
            for (int i = 0; i < tH; i++) {
                for (int j = 0; j < tW; j++) {
                    Rectangle srcRect = new Rectangle(0, 0, 16, 16);
                    if (i == 0) {
                        srcRect.Y = 0;
                    } else if (i == tH - 1) {
                        srcRect.Y = 16;
                    } else {
                        srcRect.Y = 8;
                    }
                    if (j == 0) {
                        srcRect.X = 0;
                    } else if (j == tW - 1) {
                        srcRect.X = 16;
                    } else {
                        srcRect.X = 8;
                    }
                    if (srcRect.X != 8 || srcRect.Y != 8) {
                        spriteBatch.Draw(Images.GetImage("dialogue_border"), new Rectangle(posDims.X + j * 16, posDims.Y + i * 16, 16, 16), srcRect, borderColour);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
