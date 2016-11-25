﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.Randomizers
{
    public class FakeFactory
    {
        private Random _random;
        private string[] _vowels;
        private string[] _consonants;
        private string[] _companySuffixes;

        public FakeFactory()
        {
            _random = ThreadSafeRandom.LocalRandom;
            _companySuffixes = new string[] { "Inc.", "Ltd.", "Co." };

            _vowels = new string[] { "a", "e", "i", "o", "u" };
            _consonants = new string[] { "b",
                                        "c",
                                        "d",
                                        "f",
                                        "g",
                                        "h",
                                        "j",
                                        "k",
                                        "l",
                                        "m",
                                        "n",
                                        "p",
                                        "qu",
                                        "r",
                                        "s",
                                        "t",
                                        "v",
                                        "w",
                                        "x",
                                        "y",
                                        "z"
            };
        }

        public string Company => this.ProperNoun + " " + this.CompanySuffix;
        public string CompanySuffix => _companySuffixes[_random.Next(_companySuffixes.Length - 1)];
        public string FileName => $"{ProperNoun}.{Letter}{Letter}{Letter}";
        public string Letter => _vowels.Concat(_consonants).RandomItem();


        public string FileVersion => Int(1, 9)
                               + $".{Int(0, 999):00#}"
                               + $".{Int(0, 9999):000#}";


        public string MarketSection
            => RandomItem("Dry A", "Dry B", "Dry Expansion", "Wet A", "Wet B", "Wet Expansion", 
                          "Plaza", "Night Market", "Tiangge", "Freezer");


        public string MarketStall(string sectionName)
        {
            var prefx = new string(sectionName.Trim().Split(' ').Select(x => x[0]).ToArray());
            return $"{prefx} {Int(1, 200):000}";
        }


        private string RandomItem(params string[] items) => items.RandomItem();


        public string PrintCompany
        {
            get
            {
                string propr = this.ProperNoun;
                if (this.Truthy) propr += " " + this.ProperNoun;

                var suffixes = new string[] {
            "Arts & Graphics Services",
            "Documents Solutions Inc.",
            "Graphics and Arts Printing Services",
            "Print and Design Studio",
            "Printing",
            "Printing Co., Inc.",
            "Printing Company, Inc.",
            "Printing Press",
            "Printing Services, Inc.",
            "Prints",
        };

                return propr + " " + suffixes.RandomItem();
            }
        }

        public string Vowel => _vowels[_random.Next(_vowels.Length - 1)];
        public string Consonant => _consonants[_random.Next(_consonants.Length - 1)];

        private string V => this.Vowel;
        private string C => this.Consonant;

        public string Syllable
        {
            get
            {
                switch (_random.Next(0, 14))
                {
                    case 0: return C + V;
                    case 1: return C + V;
                    case 2: return C + V;
                    case 3: return C + V;
                    case 4: return C + V;
                    case 5: return C + V + C;
                    case 6: return C + V + C;
                    case 7: return C + V + C;
                    case 8: return C + V + C;
                    case 9: return C + V + C;
                    case 10: return V + C;
                    case 11: return V + C;
                    case 12: return V + C;
                    case 13: return V;
                    case 14: return C;
                }
                return "";
            }
        }



        /// <summary>
        /// 1 fake word in title case
        /// </summary>
        public string ProperNoun => this.Word.ToTitle();

        public string Namespace => ProperNoun + NamespaceSuffix();


        public string FilePath 
            => FolderPath.Bslash(FolderPath, FileName);

        public string FolderPath
        {
            get
            {
                var drv = _vowels.Concat(_consonants).RandomItem();

                var subs = new List<string>();
                for (int i = 0; i < Int(1, 5); i++)
                    subs.Add(Word);

                return $@"{drv}:\{string.Join("\\", subs)}";
            }
        }


        private string NamespaceSuffix()
        {
            var s = ".";

            if (Truthy) s += ProperNoun + ".";

            return s + new string[] {
                "Core",
                "iOS",
                "Android",
                "Tools",
                "WinTools",
                "WpfTools",
                "BusinessLogic",
                "DataAccess",
                "Security",
            }.RandomItem();
        }



        /// <summary>
        /// Random word consisting of 2 or 3 syllables.
        /// </summary>
        public string Word
        {
            get
            {
                var str = this.Syllable + this.Syllable;
                if (this.Truthy) str += this.Syllable;
                return str;
            }
        }


        /// <summary>
        /// 3 fake words
        /// </summary>
        public string Text => this.Word + " " + this.Word + " " + this.Word;

        public string Email => this.Word + "@" + this.Word + "." + this.Syllable;
        public string Street => "#" + this.Int(1, 999) + " " + this.ProperNoun + " St.";

        public string School
        {
            get
            {
                string propr = this.ProperNoun + " " + this.ProperNoun;
                if (this.Truthy)
                    propr += " " + this.ProperNoun;
                else
                    if (this.Chance(1, 4)) propr = "St. " + propr;

                string[] suffixes = { "Academy"
                            , "Montesorri School"
                            , "Parochial School"
                            , "University"
                            , "High School"
                            , "Elementary School"
                            , "College" };

                return propr + " " + suffixes.RandomItem();
            }
        }



        public bool Truthy => this.Int(0, 1) == 1;
        public bool Chance(int numerator, int denominator) => this.Int(1, denominator) == numerator;

        public string Phone
        {
            get
            {
                var area = _random.Next(2, 99);
                var part1 = _random.Next(100, 999);
                var part2 = _random.Next(1000, 9999);
                return $"({area}) {part1}-{part2}";
            }
        }


        /// <summary>
        /// Returns random number between 0.0 and 1.0.
        /// For Percentage, Rate and Share Percentage and etc.
        /// </summary>
        public decimal Decimal => Convert.ToDecimal(this.Double);

        public double Double => _random.NextDouble();
        public byte[] Bytes => Encoding.UTF8.GetBytes(this.Word);
        public T Pick1<T>(params T[] args) => args.RandomItem();


        public DateTime BirthDate
        {
            get
            {
                int yrNow = DateTime.Today.Year;
                var minDate = new DateTime(yrNow - 65, 1, 1);
                var maxDate = new DateTime(yrNow - 25, 1, 1);

                int dayRange = (maxDate - minDate).Days;

                return minDate.AddDays(_random.Next(dayRange));
            }
        }

        public int Int(int minValue, int maxValue)
        {
            if (maxValue == int.MaxValue)
                maxValue -= 1;

            try
            {
                return _random.Next(minValue, maxValue + 1);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"‹{GetType().Name}›.Int(){L.f}" + ex.Message);
            }
        }






        //public decimal Money { get 
        //{ return this.Money(1, 1000000); }}

        public decimal Money(int min, int max)
        {
            var wholeNum = this.Int(min, max);
            return Math.Round(wholeNum + this.Decimal, 2);
        }


        public DateTime Date(int minYr, int maxYr)
        {
            var minDate = new DateTime(minYr, 1, 1);
            var maxDate = new DateTime(maxYr, 1, 1);

            int dayRange = (maxDate - minDate).Days;

            return minDate.AddDays(_random.Next(dayRange));
        }



        public string       FirstName      ()          => RandomAmerican.FirstName();
        public string       Male1stName    ()          => RandomAmerican.Male1stName();
        public string       Female1stName  ()          => RandomAmerican.Female1stName();
        public string       LastName       ()          => RandomAmerican.LastName();
        public string       FullName       ()          => RandomAmerican.FullName();
        public List<string> FirstNames     (int count) => RandomAmerican.FirstNames(count);
        public List<string> Male1stNames   (int count) => RandomAmerican.Male1stNames(count);
        public List<string> Female1stNames (int count) => RandomAmerican.Female1stNames(count);
        public List<string> LastNames      (int count) => RandomAmerican.LastNames(count);
    }


    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random _localRandom;

        // from http://stackoverflow.com/a/1262619/3973863
        public static Random LocalRandom => _localRandom ?? (_localRandom
            = new Random(unchecked(Environment.TickCount * 31)));


        public static T RandomItem<T>(this IEnumerable<T> list)
            => list.ElementAt(LocalRandom.Next(list.Count() - 1));


        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = LocalRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
