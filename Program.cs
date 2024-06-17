using System;

namespace Ludo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Solicita ao usuário a quantidade de jogadores (2-4)
            Console.WriteLine("Escolha a Quantidade de jogadores (2-4):");
            int quantidadeDeJogadores = int.Parse(Console.ReadLine()); // Pede ao usuário a quantidade de jogadores

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

            Tabuleiro tabuleiro = new Tabuleiro(); // Instancia um novo tabuleiro

            int jogadorAtual = 0; // Define o jogador atual como o primeiro da lista
            bool jogoAtivo = true; // Variável para controlar se o jogo está ativo

            while (jogoAtivo)
            {
                Console.Clear(); // Limpa a tela no início do turno de cada jogador

                Jogadores jogador = jogadores[jogadorAtual]; // Define o jogador atual
                Console.WriteLine($"Vez do jogador {jogador.Cor}"); // Mostra qual jogador está jogando

                bool jogarNovamente = true; // Variável para controlar se o jogador deve jogar novamente

                while (jogarNovamente)
                {
                    int resultadoDado = tabuleiro.Dados(); // Gera um número aleatório de 1 a 6 para simular o dado
                    Console.WriteLine($"Resultado do dado: {resultadoDado}"); // Mostra o resultado do dado

                    // Verifica se todos os peões do jogador estão na base e ele não tirou 6
                    if (TodosPeoesNaBase(jogador) && resultadoDado != 6)
                    {
                        Console.WriteLine("Todos os peões estão na base e você não tirou 6. Sua vez foi pulada.");
                        jogarNovamente = false; // Define que o jogador não joga novamente
                    }
                    else
                    {
                        // Se tirou um 6, permite mover um peão para fora da base ou movê-lo na trilha
                        if (resultadoDado == 6)
                        {
                            Console.WriteLine("Você tirou um 6! Escolha um peão para mover para fora da base ou mova um peão existente.");
                        }
                        else
                        {
                            Console.WriteLine("Escolha um peão para mover.");
                        }

                        MostrarPeoes(jogador); // Mostra a posição dos peões do jogador
                        int peaoEscolhido = int.Parse(Console.ReadLine()); // Solicita ao jogador que escolha um peão

                        // Verifica se o peão escolhido é válido e se pode ser movido
                        while (peaoEscolhido < 1 || peaoEscolhido > 4 || (resultadoDado != 6 && jogador[peaoEscolhido - 1] == -1))
                        {
                            Console.WriteLine("Peão inválido. Escolha novamente:");
                            peaoEscolhido = int.Parse(Console.ReadLine());
                        }

                        // Se tirou um 6 e o peão escolhido está na base, move-o para o início da trilha
                        if (resultadoDado == 6 && jogador[peaoEscolhido - 1] == -1)
                        {
                            jogador[peaoEscolhido - 1] = 0;
                        }
                        else
                        {
                            // Verifica se o peão está no caminho da vitória
                            if (jogador.EstaNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]))
                            {
                                int novaPosicaoNoCaminho = jogador.PosicaoNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]) + resultadoDado;
                                if (novaPosicaoNoCaminho <= 6)
                                {
                                    jogador[peaoEscolhido - 1] = 52 + novaPosicaoNoCaminho;
                                }
                                else
                                {
                                    jogador[peaoEscolhido - 1] = 52 + (6 - (novaPosicaoNoCaminho - 6)); // Volta ao início do caminho da vitória
                                    Console.WriteLine($"Você ultrapassou a casa 6. Peão {peaoEscolhido} voltou para a casa {jogador[peaoEscolhido - 1] - 52} do caminho da vitória.");
                                }
                            }
                            else
                            {
                                // Calcula a nova posição do peão na trilha
                                int novaPosicao = (jogador[peaoEscolhido - 1] + resultadoDado);
                                if (novaPosicao <= 52)
                                {
                                    jogador[peaoEscolhido - 1] = novaPosicao == 0 ? 52 : novaPosicao;
                                }


                                else
                                {
                                    // Verifica se o peão pode entrar no caminho da vitória
                                    int posicaoNoCaminhoDaVitoria = novaPosicao - 52;
                                    if (posicaoNoCaminhoDaVitoria <= 6)
                                    {
                                        jogador[peaoEscolhido - 1] = 52 + posicaoNoCaminhoDaVitoria;
                                    }
                                    else
                                    {
                                        jogador[peaoEscolhido - 1] = novaPosicao - 52; // Volta ao início da trilha se ultrapassar o caminho da vitória
                                    }
                                }
                            }
                        }

                        Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} está na posição {jogador[peaoEscolhido - 1]}");

                        // Verifica se o peão entrou no caminho da vitória
                        if (jogador.EstaNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]))
                        {
                            int posicaoNoCaminho = jogador.PosicaoNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]);
                            Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} está no caminho da vitória, na casa {posicaoNoCaminho}");

                            // Verifica se o peão alcançou a casa 6 do caminho da vitória
                            if (posicaoNoCaminho == 6)
                            {
                                jogador.TornarPeaoIndisponivel(peaoEscolhido - 1);
                                Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} chegou ao final e está indisponível.");
                            }
                        }

                        // Verifica se o jogador ganhou o jogo
                        if (jogador.GanhouOJogo())
                        {
                            Console.WriteLine($"O jogador {jogador.Cor} ganhou o jogo!");
                            jogoAtivo = false;
                            break;
                        }

                        // Se não tirou 6, para de jogar novamente
                        if (resultadoDado != 6)
                        {
                            jogarNovamente = false;
                        }
                    }
                }

                // Pausa o jogo para o próximo jogador
                if (jogoAtivo)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para passar a vez...");
                    Console.ReadLine();

                    jogadorAtual = (jogadorAtual + 1) % quantidadeDeJogadores; // Passa a vez para o próximo jogador
                }
            }
        }

        // Verifica se todos os peões do jogador estão na base
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

        // Mostra a posição atual de todos os peões do jogador
        static void MostrarPeoes(Jogadores jogador)
        {
            Console.WriteLine("Peões do jogador " + jogador.Cor + ":");
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Peão {i + 1}: ");
                if (jogador[i] == -1)
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

        // Classe que representa um jogador
        class Jogadores
        {
            private string cor; // Cor do jogador
            private int[] peoes = { -1, -1, -1, -1 }; // Posição dos peões do jogador
            private bool[] peoesIndisponiveis = { false, false, false, false }; // Indisponibilidade dos peões

            public Jogadores(string cor)
            {
                this.cor = cor; // Inicializa a cor do jogador
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

            // Indexador para acessar a posição de um peão específico
            public int this[int i]
            {
                get { return peoes[i]; }
                set { peoes[i] = value; }
            }

            // Verifica se o peão está no caminho da vitória
            public bool EstaNoCaminhoDaVitoria(int posicao)
            {
                return posicao > 52 && posicao <= 58; // Caminho da vitória entre 53 e 58
            }

            // Retorna a posição do peão no caminho da vitória
            public int PosicaoNoCaminhoDaVitoria(int posicao)
            {
                return posicao - 52; // Caminho da vitória começa na posição 53
            }

            // Torna um peão indisponível
            public void TornarPeaoIndisponivel(int peao)
            {
                peoesIndisponiveis[peao] = true;
            }

            // Verifica se o jogador ganhou o jogo
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

        // Classe que representa o tabuleiro de jogo
        class Tabuleiro
        {
            Random r = new Random(); // Gerador de números aleatórios

            // Simula o lançamento de um dado, retornando um número de 1 a 6
            public int Dados()
            {
                int d = r.Next(1, 7); // Gera um número aleatório entre 1 e 6
                return d;
            }
        }
    }
}
