using DoppelgangstersOnline.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace DoppelgangstersOnline.GameComponents
{
    public enum Class
    {
        Captain = 0,
        Security = 1,
        Engineer = 2,
        Trapper = 3,
        Hacker = 4,
        Psyonic = 5,
        Solder = 6,
        Physicist = 7,
        Agent = 8,
        Biologist = 9,
        Medic = 10,
        Operator = 11
    }

    public class Room
    {
        public List<Player> players { get; set; }
        public string RoomId { get; set; }

        //game status
        private int hacking = 0;
        private int sos = 0;
        private bool hasCard = false;
        private Timer timer;

        protected internal void GenId()
        {
            var rand = new Random();
            for (int ctr = 0; ctr <= 4; ctr++)
                RoomId = RoomId + rand.Next(0, 10).ToString();
        }

        public Room()
        {
            GenId();
            timer = new Timer();
            timer.AutoReset = false;
            players = new List<Player>();
        }

        private void Action()
        {

        }

        private void NightTimeIsOut(Object source, ElapsedEventArgs e)
        {
            timer.Stop();
            Action();
            SendMessageAll("dn");
            timer.Elapsed -= NightTimeIsOut;
            timer.Interval = 10 * 60000;
            timer.Elapsed += DayTimeIsOut;
            timer.Start();
        }

        private void DayTimeIsOut(Object source, ElapsedEventArgs e)
        {
            timer.Stop();
            SendMessageAll("dn");
            timer.Elapsed -= DayTimeIsOut;
            timer.Interval = 2 * 60000;
            timer.Elapsed += NightTimeIsOut;
            timer.Start();
        }

        protected internal void GameStart()
        {
            RolesDistribution();
            hacking = 0;
            sos = 0;
            hasCard = false;
            timer.Interval = 2 * 60000;
            timer.Elapsed += NightTimeIsOut;
            timer.Start();
        }

        private void RolesDistribution()
        {
            List<int> playersNum = new List<int>();
            for (int i = 0; i < players.Count; i++)
            {
                playersNum.Add(i);
            }

            var rand = new Random();
            for (int i = 0; i < players.Count; i++)
            {
                int n = rand.Next(0, playersNum.Count);
                players[playersNum[n]].role = (Class)i;
                playersNum.Remove(n);

                SendMessage($"csRole#{i}", players[n].NickName);
            }
        }

        //-----------------------------------------------//

        protected internal void RoomConnect(Player client)
        {
            players.Add(client);
        }

        protected internal void RoomDisconnect(Player client)
        {
            players.Remove(client);
        }

        // send message to all in room
        protected internal void SendMessageAll(string data)
        {

        }

        // send message to target
        protected internal void SendMessage(string data, string username)
        {

        }
    }
}
