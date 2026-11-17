using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace KurumsalHafizaTakip.Hubs
{
    public class BildirimHub : Hub
    {
        // Özel birine mesaj atıldığında (Sohbet sayfasında kullanılabilir)
        public async Task OzelMesajGonder(string aliciId, string gonderenAd, string mesaj, string? dosyaYolu)
        {
            await Clients.User(aliciId).SendAsync("MesajAl", gonderenAd, mesaj, dosyaYolu, DateTime.Now.ToString("HH:mm"));
        }

        // İleride admin panelinden "Tüm Sisteme Duyuru Yap" butonu eklersen kullanabileceğin metod
        public async Task TumSistemeDuyuruYap(string mesaj)
        {
            // Tüm bağlı kullanıcılardaki mavi bildirim kutusunu tetikler
            await Clients.All.SendAsync("BildirimAl", $"📢 SİSTEM DUYURUSU: {mesaj}");
        }
    }
}