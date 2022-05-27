using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public class Abiturient : IComparable<Abiturient>
    {

        public int Id { get; set; }
        public string FIO { get; set; }
        public int Sex { get; set; }
        public int Result { get; set; }
        public int Lgota { get; set; }
        public int[] Specialities { get; set; }
        public Abiturient() { }
        public Abiturient(Abiturient a)
        {
            Id = a.Id; Result = a.Result; Specialities = a.Specialities;
        }
        public int CompareTo(Abiturient a)
        {
            return a.Result.CompareTo(Result);
        }
        public static bool Contains(Abiturient[] a, Abiturient b)
        {
            bool res = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Id == b.Id)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, List<int>> lgots = new Dictionary<int, List<int>>
            {
                [1] = new List<int> { 2 },
                [2] = new List<int> { 2 },
            };

            List<Abiturient> ab = new List<Abiturient>
            {
                new Abiturient { Id = 1, FIO = "Иванов", Sex = 1, Lgota = 1, Result = 180, Specialities = new int[]{ 2, 1 } }, // 2
                new Abiturient { Id = 2, FIO = "Петров", Sex = 1, Lgota = 1, Result = 360, Specialities = new int[]{ 1, 0 } }, // 2
                new Abiturient { Id = 3, FIO = "Сидоров", Sex = 1, Lgota = 2, Result = 180, Specialities = new int[]{ 1, 2 } }, // 2
                new Abiturient { Id = 4, FIO = "Смирнов", Sex = 1, Lgota = 1, Result = 237, Specialities = new int[]{ 1, 2 } }, // 2
                new Abiturient { Id = 5, FIO = "Хващевский", Sex = 1, Lgota = 2, Result = 250, Specialities = new int[]{ 1, 0 } }, // 2
                new Abiturient { Id = 6, FIO = "Сидоров", Sex = 1, Lgota = 0, Result = 180, Specialities = new int[]{ 1, 2 } }, // 2
                new Abiturient { Id = 7, FIO = "Сидоров", Sex = 1, Lgota = 0, Result = 180, Specialities = new int[]{ 1, 2 } }, // 2
            };

            ab.Sort();

            List<Abiturient> intermediate = new List<Abiturient>();
            List<Abiturient> failed = new List<Abiturient>();
            List<int> success = new List<int>();

            List<Abiturient> sp1 = new List<Abiturient>(3);
            List<Abiturient> sp2 = new List<Abiturient>(2);
            Dictionary<string, List<Abiturient>> dict = new Dictionary<string, List<Abiturient>>()
            {
                ["sp1"] = sp1,
                ["sp2"] = sp2,
            };

            List<Abiturient> lgota = ab.Where(a => a.Lgota == 1).ToList();
            Console.WriteLine(lgota.Count);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < lgota.Count; j++)
                {
                    int sp = lgota[j].Specialities[i];
                    Dictionary<string, List<Abiturient>> abiturients = dict;
                    if (sp != 0 && lgots[sp].Contains(lgota[j].Specialities[i]))
                    {
                        if (abiturients["sp" + sp].Count < Math.Round(abiturients["sp" + sp].Capacity * 0.3))
                        {
                            if (!success.Contains(lgota[j].Id))
                            {
                                abiturients["sp" + sp].Add(lgota[j]);
                                success.Add(lgota[j].Id);
                            }
                        }
                        else
                        {
                            int minResSp = abiturients["sp" + sp].Min(a => a.Result);
                            if (lgota[j].Result > minResSp && !success.Contains(lgota[j].Id))
                            {
                                Abiturient a = abiturients["sp" + sp].Where(a => a.Result == minResSp).LastOrDefault();
                                abiturients["sp" + sp].Remove(a);
                                abiturients["sp" + sp].Add(lgota[j]);
                                success.Remove(a.Id);
                                success.Add(lgota[j].Id);
                            }

                        }
                    }
                }
            }

            dict["sp1"].Add(new Abiturient { Id = 99999, FIO = "Сидоров", Sex = 1, Lgota = 0, Result = 500, Specialities = new int[] { 1, 2 } });

            Console.WriteLine("Spec1");
            for (int i = 0; i < dict["sp1"].Count; i++)
            {
                Console.WriteLine(dict["sp1"][i].Id + " - " + dict["sp1"][i].Result);
            }
            Console.WriteLine(" ----------------------------- ");
            Console.WriteLine("Spec2");
            for (int i = 0; i < dict["sp2"].Count; i++)
            {
                Console.WriteLine(dict["sp2"][i].Id + " - " + dict["sp2"][i].Result);
            }


            Console.WriteLine(" ----------------------------- ");
            Console.WriteLine("Success");
            for (int i = 0; i < success.Count; i++)
            {
                Console.WriteLine(success[i]);
            }

            Console.WriteLine(" ----------------------------- ");
            Console.WriteLine("Failed");
            for (int i = 0; i < failed.Count; i++)
            {
                Console.WriteLine(failed[i].Id);
            }

            Console.ReadKey();
        }
    }
}

