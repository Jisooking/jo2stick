using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//123213
namespace TextRPG.Context
{
    public class CharacterJob
    {
        int level = 0;
        List<string> jobList = new List<string>(); // 전직 리스트
        public void ShowJobMsg()
        {
            bool showJobMsg = false; // 전직가능한 곳에서 마을 or 상태창?? 정도
            /* if(stat.strength >= 40)  //전직 가능 여부 표시 능력치 도달하지 못할 시 리스트 보이지않게
             * {
             *  string showGetWarrior = "워리어 전직 가능"; // 전직 문구
             *  Add.jobList(showGetWarrior); // 전직 리스트에 추가
             * }
             * if(stat.intelligence >= 40)
             * {
             *  string showGetMage = "메이지 전직 가능";
             *  Add.jobList(showGetMage);
             * }
             * if(stat.dexterity >= 40)
             * {
             *  sting showGetArcher = "아처 전직 가능";
             *  Add.jobList(showGetArcher);
             *  }
             * if(stat.agility >= 40)
             * {
             *  string showGetThief = "도적 전직 가능";
             *  Add.jobList(showGetThief); 
             * }
             */
            if (level >= 15)
            {
                Console.WriteLine("전직을 하실 수 있습니다."); // 전직 가능
                showJobMsg = true;  // 전직 창 보이게 하기
                ShowJobList(); // 전직 리스트 출력
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int num)) // || input < 0 || input jobList.Count) // 예외처리
                {
                    Console.WriteLine("올바른 입력이 아닙니다.");
                }
                else
                {
                    switch (jobList[num - 1]) // 전직 리스트에서 선택한 직업
                    {
                        case "워리어":
                            Console.WriteLine("워리어로 전직하셨습니다."); // 전직 퀘스트도 괜찮을듯.
                            GetWarrior(); // 전직 완료
                            break;
                        case "메이지":
                            Console.WriteLine("메이지로 전직하셨습니다.");
                            GetMage();
                            break;
                        case "아처":
                            Console.WriteLine("아처로 전직하셨습니다.");
                            GetArcher();
                            break;
                        case "도적":
                            Console.WriteLine("도적으로 전직하셨습니다.");
                            GetThief();
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("전직을 하실 수 없습니다."); // 전직 불가능
                showJobMsg = false; // 전직 창 보이지 않게 하기
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
        public interface IJob   // 직업 인터페이스
        {
            void ShowSkillList(); // 스킬 리스트 출력
        }
        void GetWarrior() // 전직 완료
        {
            //Character.Job = "워리어"; // 직업 변경
            //stat.strength += 10; // 능력치 변경 or 능력치 추가
            //stat.intelligence -= 5; 
            //stat.dexterity -= 5;
            //stat.agility -= 5;
            // 공격 계산식이나, 스탯별로 반영되는 비율도 조정되면 좋을것 같음.            
        }
        void GetMage()
        {
        }
        void GetArcher()
        {
        }
        void GetThief()
        {
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