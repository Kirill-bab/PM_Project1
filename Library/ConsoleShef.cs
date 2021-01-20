using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library
{
    public static class ConsoleShef
    {
        private static bool _isActive;
        private static string _emotion;
        private static bool _moodIsChanged;
        private static bool _isLoookingLeft;        
        private static Dictionary<string, string> _brows = new Dictionary<string, string>
        {
            ["regular"] = "  |---  ---|",
            ["regular_left"] = "  |--  --- |",
            ["surprized"] = "  | ^    ^ |",
            ["surprized_left"] = "  |^    ^  |",
            ["puzzled"] = "  |---  ___|",
            ["puzzled_left"] = "  |--  ___ |",
            ["angry"] = @"  | \    / |",
            ["angry_left"] = @"  |\    /  |",
            ["grumpy"] = "  |___  ___|",
            ["grumpy_right"] = "  | ___  __|",
            ["lost"] = @"  | /    \ |"
        };
        private static Dictionary<string, string> _eyes = new Dictionary<string, string>
        {
            ["regular"] = "  ||o|  |o||",
            ["left"] = "  |o|  |o| |",
            ["right"] = "  | |o|  |o|",
            ["happy"] = "  | ^    ^ |",            
            ["dead"] = "  ||x|  |x||",
            ["advicing"] = "  ||o|  |-||",
            ["closed"] = "  ||-|  |-||",
            ["closed_left"] = "  |-|  |-| |"
        };

        private static List<string> _emotions = new List<string>
        { 
            "regular",
            "happy",
            "sad",
            "indifferent",
            "dead",
            "puzzled",
            "advicing",
            "angry",
            "doubtful",
            "surprized",
            "blinking"
        };

        public static void Activate()
        {
            _isActive = true;
            ChangeMood("regular");
            Animate();
        }

        public static void Disactivate()
        {
            if (_isActive)
            {
                ChangeMood("blinking");
                _isActive = false;
            }
        }

        public static void ChangeMood(string mood)
        {
            if (_emotions.Contains(mood.ToLower()))
            {
                _emotion = mood;
                _moodIsChanged = true;
            }
            System.Threading.Thread.Sleep(100);
        }

        private async static void Animate()
        {
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(100);
                while (_isActive)
                {                   
                    string eyes = "", brows = "";
                    string[] shefImage =
                    {
                 "      __      ",
                @" .-__/  \__-. ",
                @"/   |    |   \",
                @"\   |    |   /",
                @" |          |",
                 " |__________|",
                  eyes,
                  brows,
                @"   \ ~__~ /",
                @"    \____/"
                };
                    if (_moodIsChanged)
                    {
                        switch (_emotion)
                        {
                            case "regular":
                                eyes = _eyes["regular"];
                                brows = _brows["regular"];
                                _isLoookingLeft = false;
                                break;
                            case "happy":
                                eyes = _eyes["happy"];
                                brows = _brows["regular"];
                                _isLoookingLeft = false;
                                break;
                            case "sad":
                                eyes = _eyes["regular"];
                                brows = _brows["grumpy"];
                                _isLoookingLeft = false;
                                break;
                            case "indifferent":
                                eyes = _eyes["right"];
                                brows = _brows["grumpy_right"];
                                _isLoookingLeft = false;
                                break;
                            case "dead":
                                eyes = _eyes["dead"];
                                brows = _brows["lost"];
                                _isLoookingLeft = false;
                                break;
                            case "puzzled":
                                eyes = _eyes["regular"];
                                brows = _brows["puzzled"];
                                _isLoookingLeft = false;
                                break;
                            case "advicing":
                                eyes = _eyes["advicing"];
                                brows = _brows["surprized"];
                                _isLoookingLeft = false;
                                break;
                            case "angry":
                                eyes = _eyes["regular"];
                                brows = _brows["angry"];
                                _isLoookingLeft = false;
                                break;
                            case "doubtful":
                                eyes = _eyes["left"];
                                brows = _brows["puzzled_left"];
                                _isLoookingLeft = true;
                                break;
                            case "surprized":
                                eyes = _eyes["left"];
                                brows = _brows["surprized_left"];
                                _isLoookingLeft = true;
                                break;
                            case "blinking":
                                if(_isLoookingLeft)
                                    eyes = _eyes["closed_left"];
                                else
                                    eyes = _eyes["closed"];
                                break;
                        }
                        
                        shefImage[6] = brows;
                        shefImage[7] = eyes;

                        int x = 100, y = 1, startX = Console.CursorLeft, startY = Console.CursorTop;
                        foreach (var str in shefImage)
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write(str);
                            y++;
                        }
                        _moodIsChanged = false;
                        Console.SetCursorPosition(startX, startY);
                    }   
                    if(DateTime.Now.Second % 8 == 0 && DateTime.Now.Second >= 8)
                    {
                        Blink();
                        System.Threading.Thread.Sleep(1000);
                        _moodIsChanged = true;
                    }
                }
            });
        }
        public static void Say(string message, bool clear = false)
        {
            if (_isActive)
            {
                System.Threading.Thread.Sleep(100);
                message = message.ToUpper();
                var mes = new string[message.Length / 22 + 1];
                for (int i = 0, length = 0; length < message.Length; i++)
                {
                    for (int j = length; length < j + 22 && length < message.Length; length++)
                    {
                        mes[i] += message[length];
                    }
                }
                int startX = Console.CursorLeft, startY = Console.CursorTop;
                int y = 11;
                int sleep = clear ? 0 : 100;
                foreach (string str in mes)
                {
                    Console.SetCursorPosition(95, ++y);
                    foreach (char c in str)
                    {
                        Console.Write(c);
                        System.Threading.Thread.Sleep(sleep);
                    }
                }
                System.Threading.Thread.Sleep(1000);
                if (!clear) Say(new string(' ', message.Length), true);
                Console.SetCursorPosition(startX, startY);
            }
        }
        private static async void Blink()
        {
            await Task.Run(() =>
            {
                int startX = Console.CursorLeft, startY = Console.CursorTop;
                int y = 8;
                int x = _isLoookingLeft ? 103 : 104;

                Console.SetCursorPosition(x, y);
                Console.Write("-");
                x += 5;
                Console.SetCursorPosition(x, y);
                Console.Write("-");
                Console.SetCursorPosition(startX, startY);
            });
        }
    }
}
