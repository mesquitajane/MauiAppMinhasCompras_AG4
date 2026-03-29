using MauiAppMinhasCompras.Models;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        try
        {
            var produtos = await App.Db.GetAll();

            var relatorio = produtos
                .Where(p => !string.IsNullOrEmpty(p.Categoria))
                .GroupBy(p => p.Categoria)
                .Select(g => new RelatorioItem
                {
                    Categoria = g.Key,
                    TotalGasto = g.Sum(p => p.Total)
                })
                .OrderByDescending(r => r.TotalGasto)
                .ToList();

            ListaRelatorio.ItemsSource = relatorio;

            double totalGeral = produtos.Sum(p => p.Total);
            LblTotalGeral.Text = $"Total Geral: R$ {totalGeral:F2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

