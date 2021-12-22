using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoppelgangstersOnline.Dtos;
using DoppelgangstersOnline.GameComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DoppelgangstersOnline.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, Room> _rooms;
        private readonly IDictionary<string, Player> _clients;

        public GameHub(IDictionary<string, Room> rooms, IDictionary<string, Player> clients)
        {
            _botUser = "Game";
            _rooms = rooms;
            _clients = clients;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_clients.TryGetValue(Context.ConnectionId, out Player player))
            {
                if (_rooms.TryGetValue(player.RoomId, out Room room))
                {
                    RoomDisconnect(player, room);
                    if (room.players.Count == 0)
                    {
                        _rooms.Remove(room.RoomId);
                    }
                }
            }
            return base.OnDisconnectedAsync(exception);
        }

        [Authorize]
        public async Task RoomCreate(string NickName)
        {
            // create room
            Room room = new Room();
            _rooms[room.RoomId] = room;
            // connect to server
            Player player = new Player(NickName, room.RoomId, Context.ConnectionId);
            player.roomCreator = true;
            _clients[Context.ConnectionId] = player;
            // connect to room
            room.RoomConnect(player);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);

            await Clients.Group(player.RoomId).SendAsync("RecieveRoomId", $"{room.RoomId}");
            await Clients.Group(player.RoomId).SendAsync("RecieveBotMessage",
                $"{player.NickName} has joined {player.RoomId}");

            await SendConnectedUsers(player.RoomId);
        }

        [Authorize]
        public async Task RoomConnect(string NickName, string roomId)
        {
            // get room
            if (!_rooms.TryGetValue(roomId, out Room room))
                throw new ArgumentException("Wrong Id");
            // connect to server
            Player player = new Player(NickName, room.RoomId, Context.ConnectionId);
            _clients.Add(Context.ConnectionId, player);
            // connect to room
            room.RoomConnect(player);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId);

            await Clients.Group(player.RoomId).SendAsync("RecieveBotMessage",
                $"{player.NickName} has joined {player.RoomId}");

            await SendConnectedUsers(player.RoomId);
        }

        protected internal async Task RoomDisconnect(Player player, Room room)
        {
            await SendConnectedUsers(player.RoomId);

            room.RoomDisconnect(player);
            _clients.Remove(Context.ConnectionId);

            await Clients.Group(player.RoomId).SendAsync("RecieveBotMessage", $"{player.NickName} has left");
        }

        public Task SendConnectedUsers(string room)
        {
            var users = _clients.Values
                .Where(c => c.RoomId == room)
                .Select(c => c.NickName);

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }

        public async Task SendMessage(string message)
        {
            if (_clients.TryGetValue(Context.ConnectionId, out Player player))
            {
                await Clients.Group(player.RoomId)
                    .SendAsync("RecieveMessage", player.NickName, message);
            }
        }

        public async Task SendCommand(string message)
        {
            if (_clients.TryGetValue(Context.ConnectionId, out Player player))
            {
                await Clients.Group(player.RoomId)
                    .SendAsync("RecieveCommand", player.NickName, message);
            }
        }

        public async Task SendBotMessage(string message)
        {
            if (_clients.TryGetValue(Context.ConnectionId, out Player player))
            {
                await Clients.Group(player.RoomId)
                    .SendAsync("RecieveBotMessage", player.NickName, message);
            }
        }
    }
}
