using System;
using System.Text.RegularExpressions;

namespace WMKazakhstan.TrafficLight.Core.Models
{
    public struct Digit
    {
        public static readonly Digit Default = new Digit();

        private static readonly Regex Pattern = new Regex(@"^\d{7}$", RegexOptions.Compiled);

        public Digit(int state) : this()
        {
            if (state < 0 || state > 9)
                throw new ArgumentOutOfRangeException(nameof(state));

            LoadStateFromInt32(state);
        }

        public Digit(string state) : this()
        {
            if (!Pattern.IsMatch(state))
                throw new ArgumentException("state is invalid parameter.");

            LoadStateFromString(state);
        }

        public bool Section0 { get; internal set; }
        public bool Section1 { get; internal set; }
        public bool Section2 { get; internal set; }
        public bool Section3 { get; internal set; }
        public bool Section4 { get; internal set; }
        public bool Section5 { get; internal set; }
        public bool Section6 { get; internal set; }

        public static Digit operator ++(Digit digit)
        {
            var number = digit.ToNumber();
            
            number++;

            return number > 9 
                ? new Digit(9)
                : new Digit(number);
        }

        public static Digit operator -(Digit digit, int val)
        {
            if (val >= 9)
                return Constants.Zero;

            var number = digit.ToNumber();

            number -= val;

            if (number <= 0)
                return Constants.Zero;

            return new Digit(number);
        }

        public static Digit operator +(Digit digit, int val)
        {
            if (val >= 9)
                return Constants.Nine;

            var number = digit.ToNumber();

            number += val;

            if (number >= 9)
                return Constants.Nine;

            return new Digit(number);
        }

        public override string ToString()
        {
            return ToNumber().ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 12;

                hash = hash * 32 + Section0.GetHashCode();
                hash = hash * 32 + Section1.GetHashCode();
                hash = hash * 32 + Section2.GetHashCode();
                hash = hash * 32 + Section3.GetHashCode();
                hash = hash * 32 + Section4.GetHashCode();
                hash = hash * 32 + Section5.GetHashCode();
                hash = hash * 32 + Section6.GetHashCode();

                return hash;
            }
        }

        public bool MaybeEquals(Digit digit)
        {
            if (digit.Section0 && !Section0) return false;
            if (digit.Section1 && !Section1) return false;
            if (digit.Section2 && !Section2) return false;
            if (digit.Section3 && !Section3) return false;
            if (digit.Section4 && !Section4) return false;
            if (digit.Section5 && !Section5) return false;
            if (digit.Section6 && !Section6) return false;

            return true;
        }

        public int ToNumber()
        {
            return this switch
            {
                { Section0: true, Section1: true, Section2: true, Section3: false, Section4: true, Section5: true, Section6: true}     => 0,
                { Section0: false, Section1: false, Section2: true, Section3: false, Section4: false, Section5: true, Section6: false} => 1,
                { Section0: true, Section1: false, Section2: true, Section3: true, Section4: true, Section5: false, Section6: true}    => 2,
                { Section0: true, Section1: false, Section2: true, Section3: true, Section4: false, Section5: true, Section6: true}    => 3,
                { Section0: false, Section1: true, Section2: true, Section3: true, Section4: false, Section5: true, Section6: false}   => 4,
                { Section0: true, Section1: true, Section2: false, Section3: true, Section4: false, Section5: true, Section6: true}    => 5,
                { Section0: true, Section1: true, Section2: false, Section3: true, Section4: true, Section5: true, Section6: true}     => 6,
                { Section0: true, Section1: false, Section2: true, Section3: false, Section4: false, Section5: true, Section6: false}  => 7,
                { Section0: true, Section1: true, Section2: true, Section3: true, Section4: true, Section5: true, Section6: true}      => 8,
                { Section0: true, Section1: true, Section2: true, Section3: true, Section4: false, Section5: true, Section6: true}     => 9,
                
                _ => throw new InvalidOperationException("Digit is broken")                                                                                                                
            };
        }

        public string ToBitString()
        {
            var chars = new char[7];

            chars[0] = toChar(Section0);
            chars[1] = toChar(Section1);
            chars[2] = toChar(Section2);
            chars[3] = toChar(Section3);
            chars[4] = toChar(Section4);
            chars[5] = toChar(Section5);
            chars[6] = toChar(Section6);

            return new string(chars);

            static char toChar(bool section)
            {
                return section ? '1' : '0';
            }
        }

        private void LoadStateFromInt32(int number)
        {
            switch(number)
            {
                case 0:
                    SectionsSwitchTo(true);
                    Section3 = false;
                    break;

                case 1:
                    SectionsSwitchTo(false);
                    Section2 = true;
                    Section5 = true;
                    break;

                case 2:
                    SectionsSwitchTo(true);
                    Section1 = false;
                    Section5 = false;
                    break;

                case 3:
                    SectionsSwitchTo(true);
                    Section1 = false;
                    Section4 = false;
                    break;

                case 4:
                    SectionsSwitchTo(true);
                    Section0 = false;
                    Section4 = false;
                    Section6 = false;
                    break;

                case 5:
                    SectionsSwitchTo(true);
                    Section2 = false;
                    Section4 = false;
                    break;

                case 6:
                    SectionsSwitchTo(true);
                    Section2 = false;
                    break;

                case 7:
                    SectionsSwitchTo(false);
                    Section0 = true;
                    Section2 = true;
                    Section5 = true;
                    break;

                case 8:
                    SectionsSwitchTo(true);
                    break;

                case 9:
                    SectionsSwitchTo(true);
                    Section4 = false;
                    break;
            }
        }

        private void LoadStateFromString(string state)
        {
            Section0 = isTurnOn(state[0]);
            Section1 = isTurnOn(state[1]);
            Section2 = isTurnOn(state[2]);
            Section3 = isTurnOn(state[3]);
            Section4 = isTurnOn(state[4]);
            Section5 = isTurnOn(state[5]);
            Section6 = isTurnOn(state[6]);

            static bool isTurnOn(char ch)
            {
                return ch == '1';
            }
        }

        private void SectionsSwitchTo(bool state)
        {
            Section0 = state;
            Section1 = state;
            Section2 = state;
            Section3 = state;
            Section4 = state;
            Section5 = state;
            Section6 = state;
        }
    }
}
