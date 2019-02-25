using System;
using System.Collections.Generic;
using System.Linq;

namespace peselgen {
    class Program {
        private static readonly DateTime START_DATE = DateTime.Today.AddYears(-65);
        private static readonly DateTime END_DATE = DateTime.Today.AddYears(-20);

        private static readonly int[] GENDER_DIGITS = GenderDigits(Gender.Male);

        private static readonly int[] _peselDigits = new int[11];

        static void Main(string[] args) {
            foreach (var day in DaysBetween(START_DATE, END_DATE)) {
                SetPeselDay(day);
                PrintAllPeselsForDay();
            }
        }

        private static int[] GenderDigits(Gender gender) {
            switch (gender) {
                case Gender.Male:
                    return new[] { 1, 3, 5, 7, 9 };

                case Gender.Female:
                    return new[] { 0, 2, 4, 6, 8 };

                default:
                    return new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
        }

        private static void SetPeselDay(DateTime day) {
            int yearShort = day.Year % 100;
            _peselDigits[0] = yearShort / 10;
            _peselDigits[1] = yearShort % 10;

            int century = day.Year / 100;
            int monthOffset = MonthOffsetForCentury(century);
            int peselMonth = day.Month + monthOffset;
            _peselDigits[2] = peselMonth / 10;
            _peselDigits[3] = peselMonth % 10;

            int peselDay = day.Day;
            _peselDigits[4] = peselDay / 10;
            _peselDigits[5] = peselDay % 10;
        }

        private static int MonthOffsetForCentury(int century) {
            switch (century) {
                case 18: return 80;
                case 19: return 0;
                case 20: return 20;
                case 21: return 40;
                case 22: return 60;
            }

            throw new ArgumentException("Invalid centruy: " + century);
        }

        private static void PrintAllPeselsForDay() {
            for (int d1 = 0; d1 <= 9; d1++)
            for (int d2 = 0; d2 <= 9; d2++)
            for (int d3 = 0; d3 <= 9; d3++)
            for (int i = 0; i < GENDER_DIGITS.Length; i++)
            {
                int d4 = GENDER_DIGITS[i];

                _peselDigits[6] = d1;
                _peselDigits[7] = d2;
                _peselDigits[8] = d3;
                _peselDigits[9] = d4;

                _peselDigits[10] = PeselChecksum(_peselDigits);

                FastPrintPesel();
            }
        }


        private static readonly char[] _peselCharacters = new char[_peselDigits.Length];
        private static void FastPrintPesel() {
            for (int i = 0; i < _peselDigits.Length; i++) {
                _peselCharacters[i] = (char)('0' + _peselDigits[i]);
            }
            Console.WriteLine(_peselCharacters);
        }

        private static IEnumerable<DateTime> DaysBetween(DateTime start, DateTime end) {
            var curr = start;

            do {
                yield return curr;
                curr = curr.AddDays(1);
            } while (curr <= end);
        }

        private readonly static int[] CHECKSUM_WEIGHTS = { 1,3,7,9,1,3,7,9,1,3 };
        private static int PeselChecksum(int[] peselDigits) {
            var sum = 0;
            for (int i = 0; i < CHECKSUM_WEIGHTS.Length; i += 1) {
                sum += CHECKSUM_WEIGHTS[i] * peselDigits[i];
            }
            sum = (10 - (sum % 10)) % 10;

            return sum;
        }
    }

    public enum Gender {
        Male, Female, Both
    }
}
