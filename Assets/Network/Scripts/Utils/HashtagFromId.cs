using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Network
{
    public static class HashtagFromId
    {
        const string TAG = "0289PYLQGRJCUV";

        public static string Generate(string uuid)
        {
            var data = Sha1Hash(uuid);
            string bit = BitConverter.ToString(data).Replace("-", "");
            bit = bit[..9];

            var longStr = "";
            foreach (var i in bit)
            {
                longStr += System.Convert.ToInt32(i);
            }
            var longID = GetHighInt(long.Parse(longStr));
            if (GetHighInt(longID) <= 225)
            {
                StringBuilder sb = new StringBuilder();
                int cnt = 11;
                int ccnt = TAG.Length;
                longID = ((long)GetLowInt(longID) << 8) + (GetHighInt(longID));
                do
                {
                    sb.Append(TAG[(int)(longID % ccnt)]);
                    longID /= ccnt;
                    if (longID <= 0)
                    {
                        break;
                    }
                } while (++cnt > 0);
                sb.Append("#");

                return new string(sb.ToString().Reverse().ToArray());
            }

            return "";
        }

        static byte[] Sha1Hash(string uuid)
        {

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(uuid));
            }
        }
        static long GetHighInt(long n)
        {
            return n >> 32;
        }

        static long GetLowInt(long n)
        {
            return n & 0xFFFFFFFF;
        }
    }

}

