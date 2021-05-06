using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OV.MVX.Helpers
{
    public static class DocumentValidation
    {
        private const string allowedLettersForDocument = "TRWAGMYFPDXBNJZSQVHLCKET";
        public static bool isValidDocument(string documentCode)
        {
            bool isValid = false;
            if(documentCode.Length == 9)
            {
                isValid = isValidDNI(documentCode);
            } else if(documentCode.Length == 10)
            {
                isValid = isValidNIE(documentCode);
            } else
            {

            }
            return isValid;
        }

        private static bool isValidNIE(string documentCode)
        {
            const int POSITION_OF_FIRST_LETTER = 1;
            const int NUMBER_OF_NON_NUMERIC_CHARACTERS_IN_NIE = 3;
            int LENGTH_OF_NUMBERS_IN_NIE = documentCode.Length - NUMBER_OF_NON_NUMERIC_CHARACTERS_IN_NIE;
            try
            {
                var NIEfirstLetter = documentCode.ToUpper().First();
                var NIEnumbers = Int32.Parse(documentCode.Substring(POSITION_OF_FIRST_LETTER, LENGTH_OF_NUMBERS_IN_NIE));
                var NIEletter = documentCode.ToUpper().Last();
                var numberValueOfFirstNIELetter = GetNumberOfFirstNIELetter(NIEfirstLetter);
                var positionOfCorrectLetter = (NIEnumbers + numberValueOfFirstNIELetter) % 23;
                return allowedLettersForDocument[positionOfCorrectLetter] == NIEletter;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int GetNumberOfFirstNIELetter(char letter)
        {
            if(letter == 'X')
            {
                return 0;
            } else if(letter == 'Y')
            {
                return 10000000;
            } else if(letter == 'Z')
            {
                return 20000000;
            } else
            {
                throw new Exception("Invalid first character");
            }
        }

        private static bool isValidDNI(string documentCode)
        {
            try
            {
                var DNInumbers = Int32.Parse(documentCode.Remove(documentCode.Length - 1));
                var DNIletter = documentCode.ToUpper().Last();
                var positionOfCorrectLetter = DNInumbers % 23;
                return allowedLettersForDocument[positionOfCorrectLetter] == DNIletter;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
