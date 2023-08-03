using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Unipluss.Sign.ExternalContract.Entities;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.Repository;
using WebApplication1.DataAccess.Models;


namespace WebApplication1.ChatHubSignarl
{
    public class CartHub : Hub
    {

        //// update quantity của sản phẩm
        //// Define a method to broadcast cart updates to all connected clients
        //public async Task BroadcastCartUpdate()
        //{
        //    await Clients.All.SendAsync("CartUpdated");
        //}

    }
}
