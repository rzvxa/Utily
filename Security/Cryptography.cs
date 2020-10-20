using Utils.SystemTypes;
using UnityEngine;
using System;

using Math = Utils.Unity.Runtime.Math;

namespace Utils.Security
{
    public static class Cryptography
    {
        /// <summary>
        ///   ROT5 character rotation(Digits only),
        ///   Call it with encrypted string to get original back
        /// </summary>
        public static string Rot5(string input)
        {
            if (input.NullOrEmpty() || input.NullOrWhiteSpace())
                return input;

            var buffer = input.ToCharArray();
            for (int i = 0; i < buffer.Length; ++i)
                buffer[i] = (char)RotateCharacter(buffer[i], '0', '9');

            return new string(buffer);
        }

        /// <summary>
        ///   ROT13 character rotation,(a-z, A-Z)
        ///   Call it with encrypted string to get original back
        /// </summary>
        public static string Rot13(string input)
        {
            if (input.NullOrEmpty() || input.NullOrWhiteSpace())
                return input;

            var buffer = input.ToCharArray();
            for (int i = 0; i < buffer.Length; ++i)
            {
                int c = buffer[i];
                c = RotateCharacter(c, 'a', 'z');
                c = RotateCharacter(c, 'A', 'Z');
                buffer[i] = (char)c;
            }
            return new string(buffer);
        }

        /// <summary>
        ///   ROT18 character rotation,(0-9, a-z, A-Z)
        ///   Call it with encrypted string to get original back
        /// </summary>
        public static string Rot18(string input)
        {
            if (input.NullOrEmpty() || input.NullOrWhiteSpace())
                return input;

            var buffer = input.ToCharArray();
            for (int i = 0; i < buffer.Length; ++i)
            {
                int c = buffer[i];
                c = RotateCharacter(c, '0', '9');
                c = RotateCharacter(c, 'a', 'z');
                c = RotateCharacter(c, 'A', 'Z');
                buffer[i] = (char)c;
            }
            return new string(buffer);
        }

        /// <summary>
        ///   ROT47 character rotation,(!-~)
        ///   Call it with encrypted string to get original back
        /// </summary>
        public static string Rot47(string input)
        {
            if (input.NullOrEmpty() || input.NullOrWhiteSpace())
                return input;

            var buffer = input.ToCharArray();
            for (int i = 0; i < buffer.Length; ++i)
                buffer[i] = (char)RotateCharacter(buffer[i], '!', '~');

            return new string(buffer);
        }

        public enum CaesarCipherCaseStrategy
        {
            Maintain,
            Ignore,
            Strict // A != a
        }

        public static string CaesarEndrypt(string input,
                                           int shift = 5,
                                           string alphabet =
                                           "abcdefghijklmnopqrstuvwxyz0123456789",
                                           CaesarCipherCaseStrategy caseStrategy =
                                           CaesarCipherCaseStrategy.Maintain,
                                           bool includeForeginChars = true) =>
            CaesarCipher(input, true, shift, alphabet, caseStrategy, includeForeginChars);

        public static string CaesarDecrypt(string input,
                                           int shift = 5,
                                           string alphabet =
                                           "abcdefghijklmnopqrstuvwxyz0123456789",
                                           CaesarCipherCaseStrategy caseStrategy =
                                           CaesarCipherCaseStrategy.Maintain,
                                           bool includeForeginChars = true) =>
            CaesarCipher(input, false, shift, alphabet, caseStrategy, includeForeginChars);

        public static string CaesarCipher(string input,
                                          bool isEncode,
                                          int shift,
                                          string alphabet,
                                          CaesarCipherCaseStrategy caseStrategy,
                                          bool includeForeginChars)
        {
            char[] lAlphabet = alphabet.ToLower().ToCharArray();
            char[] uAlphabet = null;
            if (caseStrategy != CaesarCipherCaseStrategy.Strict)
            {
                uAlphabet = alphabet.ToUpper().ToCharArray();
            }

            int aLen = lAlphabet.Length;
            int iLen = input.Length;
            var buffer = input.ToCharArray();

            int c, x, y = 0, j = 0;
            bool u;

            // iterate input
            for (int i = 0; i < iLen; ++i)
            {
                c = buffer[i];

                // Match alphabet character
                x = Array.IndexOf(lAlphabet, buffer[i]);
                u = false;

                // Match uppercase alphabet character (depending on case strategy)
                if (x == -1 && caseStrategy != CaesarCipherCaseStrategy.Strict)
                {
                    x = Array.IndexOf(uAlphabet, buffer[i]);
                    u = true;
                }

                if (x == -1)
                {
                    // Character is not in the alphabet
                    if (includeForeginChars)
                        buffer[j++] = (char)c;
                }
                else
                {
                    // Shift character
                    y = Math.AbsMod((x + shift * (isEncode ? 1 : -1)), aLen);

                    // Translate index to character following the case strategy
                    if (caseStrategy == CaesarCipherCaseStrategy.Maintain && u)
                        buffer[j++] = uAlphabet[y];
                    else
                        buffer[j++] = lAlphabet[y];
                }
            }

            return new string(buffer);
        }

        private static int RotateCharacter(int @char, int start, int end)
        {
            // Only rotate if code point is inside bounds.
            if (@char >= start && @char <= end)
            {
                var count = end - start + 1;
                @char += count / 2;

                if (@char > end)
                    @char -= count;
            }
            return @char;
        }
    }
}
