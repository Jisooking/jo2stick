using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//123213
namespace TextRPG.Context
{
    public class CharacterStatForGetJob
    {
        public string? job {  get; set; }
        public int Level { get; set; }
        public int Str { get; set; }
        public int Int { get; set; }
        public int Dex { get; set; }
        public int Luk { get; set; }
        public CharacterStatForGetJob (SaveData savedata)
        {
            this.job = savedata.job;
            this.Level = savedata.Level;
            this.Str = savedata.Str;
            this.Int = savedata.Int;
            this.Dex = savedata.Dex;
            this.Luk = savedata.Luk;
        }
    }
    public class CharacterJob
    {
        private List<string> jobList = new List<string>();  // 전직 리스트        
        private CharacterStatForGetJob statData;

        public CharacterJob(CharacterStatForGetJob statData)
        {
            this.statData = statData;
        }
        public void ShowJobMsg(SaveData savedata)
        {
            jobList.Clear(); // 매번 초기화
                             // 전직 가능 조건 확인
            if (statData.Str >= 40) jobList.Add("워리어");
            if (statData.Int >= 40) jobList.Add("메이지");
            if (statData.Dex >= 40) jobList.Add("아처");
            if (statData.Luk >= 40) jobList.Add("도적");

            if (statData.Level >= 15 && jobList.Count > 0)
            {
                Console.WriteLine("전직을 하실 수 있습니다.");
                ShowJobList();

                Console.Write("직업 번호를 입력하세요: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int num) && num >= 1 && num <= jobList.Count)
                {
                    string selectedJob = jobList[num - 1];
                    Console.WriteLine($"{selectedJob}로 전직하셨습니다.");

                    switch (selectedJob)
                    {
                        case "워리어": GetWarrior(savedata); break;
                        case "메이지": GetMage(savedata); break;
                        case "아처": GetArcher(savedata); break;
                        case "도적": GetThief(savedata); break;
                    }
                }
                else
                {
                    Console.WriteLine("올바른 입력이 아닙니다.");
                }
            }
            else
            {
                Console.WriteLine("전직 조건을 만족하지 않습니다.");
            }
        }
        public void ShowJobList() // 전직 리스트 출력
        {
            Console.WriteLine("전직 가능 리스트");
            for (int i = 0; i < jobList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {jobList[i]}");
            }
        }      
        void GetWarrior(SaveData savedata) // 전직 완료
        {
            savedata.job = "워리어"; // 직업 변경
            savedata.Str += 20; // 능력치 변경 or 능력치 추가
            savedata.Dex += 10;
            // 공격 계산식이나, 스탯별로 반영되는 비율도 조정되면 좋을것 같음.            
        }
        void GetMage(SaveData savedata)
        {
            savedata.job = "메이지";
            savedata.Int += 20;
            savedata.Luk += 10;
        }
        void GetArcher(SaveData savedata)
        {
            savedata.job = "아처";
            savedata.Dex += 20;
            savedata.Str += 10;
        }
        void GetThief(SaveData savedata)
        {
            savedata.job = "도적";
            savedata.Luk += 20;
            savedata.Dex += 10;
        }
        public interface IJob   // 직업 인터페이스
        {
            void ShowSkillList(); // 스킬 리스트 출력
        }
        public void ShowSkillListByJob(string jobName)
        {
            IJob? job = jobName switch
            {
                "워리어" => new Warrior(),
                "메이지" => new Mage(),
                "아처" => new Archer(),
                "도적" => new Thief(),
                _ => null
            };

            if (job != null)
                job.ShowSkillList();
            else
                Console.WriteLine("잘못된 입력입니다.");
        }
        public class Warrior : IJob
        {
            public void ShowSkillList()
            {
                Console.WriteLine("워리어 스킬 리스트"); // 어떤식으로 출력할지 의논해보기
            }
        }
        public class Mage : IJob
        {
            public void ShowSkillList()
            {
                Console.WriteLine("메이지 스킬 리스트");
            }
        }
        public class Archer : IJob
        {
            public void ShowSkillList()
            {
                Console.WriteLine("아처 스킬 리스트");
            }
        }
        public class Thief : IJob
        {
            public void ShowSkillList()
            {
                Console.WriteLine("도적 스킬 리스트");
            }
        }
    }
}