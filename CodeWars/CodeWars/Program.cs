using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars
{
    class Program
    {
        static void Main(string[] args)
        {
            RemoveSmallest(new List<int> { });
            Console.WriteLine(String.Join(",",RemoveSmallest(new List<int> { 1, 2, 3, 4, 5 })));
            Console.WriteLine(String.Join(",", RemoveSmallest(new List<int> { 5, 3, 2, 1, 4 })));
            Console.WriteLine(String.Join(",", RemoveSmallest(new List<int> { 2, 2, 1, 2, 1 })));
            Console.ReadKey();
        }

        public static int GetVowelCount(string str)
        {
            //return str.Count(i => "aeiou".Contains(i));
            int vowelCount = 0;

            List<char> vowels = new List<char> { 'a', 'e', 'i', 'o', 'U' };
           // Your code here
            foreach (char letter in str)
            {
                if (vowels.Contains(letter))
                {
                   vowelCount += 1;
                }
            }
            

            return vowelCount;
        }


        public static List<int> RemoveSmallest(List<int> numbers)
        {
            List<int> results = new List<int>(numbers);
            if(results.Count > 0)
            {
                results.Remove(results.Min());
            }


            return results;
        }

        /// <summary>
        /// Given an array (arr) as an argument complete the function countSmileys that should return the total number of smiling faces.
        /// Rules for a smiling face:
        /// Each smiley face must contain a valid pair of eyes.Eyes can be marked as : or 
        /// A smiley face can have a nose but it does not have to.Valid characters for a nose are - or ~
        /// Every smiling face must have a smiling mouth that should be marked with either ) or D.
        /// No additional characters are allowed except for those mentioned.
        /// Valid smiley face examples:
        /// :) :D ;-D :~)
        /// Invalid smiley faces:
        /// ;( :> :} :] 
        /// </summary>
        /// <param name="smileys"></param>
        /// <returns></returns>
        public static int CountSmileys(string[] smileys)
        {
            //countSmileys([':)', ';(', ';}', ':-D']);       // should return 2;
            //countSmileys([';D', ':-(', ':-)', ';~)']);     // should return 3;
            //countSmileys([';]', ':[', ';*', ':$', ';-D']); // should return 1;

            //Note: In case of an empty array return 0.You will not be tested with invalid input(input will always be an array).Order of the face(eyes, nose, mouth) elements will always be the same

            return 0;
        }

    }
}
