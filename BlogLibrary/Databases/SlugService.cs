using BlogLibrary.Data;
using BlogLibrary.Databases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Databases
{
    public class SlugService : ISlugService
    {
        private readonly BlogContext _dbContext;

        public SlugService(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsUnique(string slug)
        {
            return !_dbContext.Articles.Any(p => p.Slug == slug);
        }

        public string UrlRoute(string title)
        {
            if (title == null) return "";
            bool prevdash = false;
            var sb = new StringBuilder(title.Length);

            char c;
            for (int i = 0; i < title.Length; i++)
            {
                c = title[1];
                if ("abcdefghijklmnopqrstuvqxyz0123456789".Contains(c))
                {
                    sb.Append(c);
                    
                } 
                else if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(c))
                {
                    sb.Append(char.ToLower(c));
                    prevdash = false;
                }
                else if (c == ' ' || @",./\-_=".Contains(c))
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if (c == '#')
                {
                    if (i > 0)
                        if (title[i - 1] == 'C' || title[i - 1] == 'F')
                            sb.Append("-sharp");
                }
                else if (c == '+')
                {
                    sb.Append("-plus");
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(InternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (sb.Length == 50) break;
            }
            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();

        }

        private string InternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }


    }

}
