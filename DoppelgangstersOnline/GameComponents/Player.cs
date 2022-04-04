using DoppelgangstersOnline.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoppelgangstersOnline.Dtos
{
    public class Player
    {
        public string NickName { get; set; }
        public string RoomId { get; set; }
        public string ConnectionId { get; set; }

        protected internal bool roomCreator = false;

        //player parametrs        
        private int hp = 2;
        protected internal Class role;

        private int blocked = 0;
        private int inPrison = 0;
        private int poisoned = 0;
        private int item = 0;

        private int goSleep = 0;
        private string target;

        public Player(string name, string room, string id)
        {
            NickName = name;
            RoomId = room;
            ConnectionId = id;

            hp = 2;

            blocked = 0;
            inPrison = 0;
            poisoned = 0;
            item = 0;

            goSleep = 0;
        }    
    }
}
