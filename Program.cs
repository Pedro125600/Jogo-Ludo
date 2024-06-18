using System;

namespace Ludo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Solicita ao usuário a quantidade de jogadores (2-4)
            Console.WriteLine("Escolha a Quantidade de jogadores (2-4):");
            int quantidadeDeJogadores = int.Parse(Console.ReadLine());

            // Verifica se a quantidade de jogadores está entre 2 e 4
            while (quantidadeDeJogadores < 2 || quantidadeDeJogadores > 4)
            {
                Console.WriteLine("Número inválido. Escolha uma quantidade de jogadores entre 2 e 4:");
                quantidadeDeJogadores = int.Parse(Console.ReadLine());
            }

            // Inicializa o vetor de jogadores com base na quantidade informada
            Jogadores[] jogadores = new Jogadores[quantidadeDeJogadores];

            // Adiciona jogadores Vermelho e Verde se houver pelo menos 2 jogadores
            if (quantidadeDeJogadores >= 2)
            {
                jogadores[0] = new Jogadores("Vermelho");
                jogadores[1] = new Jogadores("Verde");
            }

            // Adiciona jogador Amarelo se houver pelo menos 3 jogadores
            if (quantidadeDeJogadores >= 3)
            {
                jogadores[2] = new Jogadores("Amarelo");
            }

            // Adiciona jogador Azul se houver 4 jogadores
            if (quantidadeDeJogadores == 4)
            {
                jogadores[3] = new Jogadores("Azul");
            }

            Tabuleiro tabuleiro = new Tabuleiro();

            int jogadorAtual = 0;
            bool jogoAtivo = true;

            while (jogoAtivo)
            {
                Console.Clear();
                Jogadores jogador = jogadores[jogadorAtual];
                Console.WriteLine($"Vez do jogador {jogador.Cor}");

                bool jogarNovamente = true;

                while (jogarNovamente)
                {
                    int resultadoDado = tabuleiro.Dados();
                    Console.WriteLine($"Resultado do dado: {resultadoDado}");

                    if (TodosPeoesNaBase(jogador) && resultadoDado != 6)
                    {
                        Console.WriteLine("Todos os peões estão na base e você não tirou 6. Sua vez foi pulada.");
                        jogarNovamente = false;
                    }
                    else
                    {
                        if (resultadoDado == 6)
                        {
                            Console.WriteLine("Você tirou um 6! Escolha um peão para mover para fora da base ou mova um peão existente.");
                        }
                        else
                        {
                            Console.WriteLine("Escolha um peão para mover.");
                        }

                        MostrarPeoes(jogador);
                        Console.WriteLine("Digite 5 para pular sua vez.");
                        int peaoEscolhido = int.Parse(Console.ReadLine());

                        if (peaoEscolhido == 5)
                        {
                            Console.WriteLine("Você escolheu pular sua vez.");
                            jogarNovamente = false;
                            break;
                        }

                        while (peaoEscolhido < 1 || peaoEscolhido > 4 || (resultadoDado != 6 && jogador[peaoEscolhido - 1] == -1) || jogador.PeaoIndisponivel(peaoEscolhido - 1))
                        {
                            Console.WriteLine("Peão inválido. Escolha novamente ou digite 5 para pular sua vez:");
                            peaoEscolhido = int.Parse(Console.ReadLine());

                            if (peaoEscolhido == 5)
                            {
                                Console.WriteLine("Você escolheu pular sua vez.");
                                jogarNovamente = false;
                                break;
                            }
                        }

                        if (peaoEscolhido == 5)
                        {
                            break;
                        }

                        if (resultadoDado == 6 && jogador[peaoEscolhido - 1] == -1)
                        {
                            jogador[peaoEscolhido - 1] = 0;
                        }
                        else
                        {
                            if (jogador.EstaNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]))
                            {
                                int posicaoAtualNoCaminho = jogador.PosicaoNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]);
                                int novaPosicaoNoCaminho = posicaoAtualNoCaminho + resultadoDado;

                                if (novaPosicaoNoCaminho == 6)
                                {
                                    jogador.TornarPeaoIndisponivel(peaoEscolhido - 1);
                                    Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} chegou ao final e está indisponível.");
                                    jogarNovamente = true; // Tem direito a outra rodada
                                }
                                else if (novaPosicaoNoCaminho > 6) // se ele tirou um numero menor que 6 ele joga de novo
                                {
                                    jogarNovamente = false; // Não joga novamente, a menos que tire 6 novamente
                                }
                                else
                                {
                                    Console.WriteLine("Movimento inválido. Escolha outra peça.");
                                    jogarNovamente = false;
                                }
                            }
                            else
                            {
                                int novaPosicao = (jogador[peaoEscolhido - 1] + resultadoDado);
                                if (novaPosicao <= 52)
                                {
                                    jogador[peaoEscolhido - 1] = novaPosicao == 0 ? 52 : novaPosicao;
                                }
                                else
                                {
                                    int posicaoNoCaminhoDaVitoria = novaPosicao - 52;
                                    if (posicaoNoCaminhoDaVitoria <= 6)
                                    {
                                        jogador[peaoEscolhido - 1] = 52 + posicaoNoCaminhoDaVitoria;
                                    }
                                    else
                                    {
                                        jogador[peaoEscolhido - 1] = novaPosicao - 52;
                                    }
                                }
                                jogarNovamente = resultadoDado == 6; // Joga novamente se tirou 6
                            }
                        }

                        Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} está na posição {jogador[peaoEscolhido - 1]}");

                        if (jogador.EstaNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]))
                        {
                            int posicaoNoCaminho = jogador.PosicaoNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]);
                            Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} está no caminho da vitória, na casa {posicaoNoCaminho}");

                            if (posicaoNoCaminho == 6)
                            {
                                jogador.TornarPeaoIndisponivel(peaoEscolhido - 1);
                                Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} chegou ao final e está indisponível.");
                            }
                        }

                        if (jogador.GanhouOJogo())
                        {
                            Console.WriteLine($"O jogador {jogador.Cor} ganhou o jogo!");
                            jogoAtivo = false;
                            break;
                        }
                    }
                }

                if (jogoAtivo)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para passar a vez...");
                    Console.ReadLine();

                    jogadorAtual = (jogadorAtual + 1) % quantidadeDeJogadores;
                }
            }
        }

        static bool TodosPeoesNaBase(Jogadores jogador)
        {
            for (int i = 0; i < jogador.Peoes.Length; i++)
            {
                if (jogador.Peoes[i] != -1)
                {
                    return false;
                }
            }
            return true;
        }

        static void MostrarPeoes(Jogadores jogador)
        {
            Console.WriteLine("Peões do jogador " + jogador.Cor + ":");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Peão {i + 1}: ");
                if (jogador.PeaoIndisponivel(i))
                {
                    Console.WriteLine("Ganhou");
                }
                else if (jogador[i] == -1)
                {
                    Console.WriteLine("Na base");
                }
                else if (jogador[i] > 52)
                {
                    Console.WriteLine($"Caminho da Vitória - Casa {jogador[i] - 52}");
                }
                else
                {
                    Console.WriteLine($"Posição {jogador[i]}");
                }
            }
        }

        class Jogadores
        {
            private string cor;
            private int[] peoes = { -1, -1, -1, -1 };
            private bool[] peoesIndisponiveis = { false, false, false, false };

            public Jogadores(string cor)
            {
                this.cor = cor;
            }

            public string Cor
            {
                get { return cor; }
                set { cor = value; }
            }

            public int[] Peoes
            {
                get { return peoes; }
            }

            public int this[int i]
            {
                get { return peoes[i]; }
                set { peoes[i] = value; }
            }

            public bool EstaNoCaminhoDaVitoria(int posicao)
            {
                return posicao > 52 && posicao <= 58;
            }

            public int PosicaoNoCaminhoDaVitoria(int posicao)
            {
                return posicao - 52;
            }

            public void TornarPeaoIndisponivel(int peao)
            {
                peoesIndisponiveis[peao] = true;
            }

            public bool PeaoIndisponivel(int peao)
            {
                return peoesIndisponiveis[peao];
            }

            public bool GanhouOJogo()
            {
                for (int i = 0; i < peoesIndisponiveis.Length; i++)
                {
                    if (!peoesIndisponiveis[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        class Tabuleiro
        {
            Random r = new Random();

            public int Dados()
            {
                int d = r.Next(1, 7);
                return d;
            }
        }
    }
}
