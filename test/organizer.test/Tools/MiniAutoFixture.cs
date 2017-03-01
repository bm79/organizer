using System;
using System.Linq;


namespace Organizer.Test.Tools {
    public class MiniAutoFixture
    {
        private Random gen;
        public void Initialize()
        {
            gen = new Random();
        }

        public int CreateInt()
        {
            return Math.Abs(gen.Next(1000000));
        }


        public string CreateString(int maxLength)
        {
            int length = gen.Next(maxLength)+1;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEDFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[gen.Next(s.Length)]).ToArray());
        }

        public string CreateString()
        {
            int length = gen.Next(24)+8;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEDFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[gen.Next(s.Length)]).ToArray());
        }

        public DateTime CreateDatetime()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;           
            return start.AddDays(gen.Next(range));
        }

        public bool CreateBoolean()
        {
           return gen.Next(1000000)%2==0;
        }
    }
}