using System;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.viewModel
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public double Price { get; set; }
    }
}