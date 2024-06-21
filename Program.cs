using System;
using System.Text;
using System.IO;

namespace Ludo
{
    // Faça com que a cada fez que o jogador tirar 6 salva este numero no vetor se ele tirar um segundo 6 salva ele de novo e na segunda vez se ele tirar um 6 novamente pula a fez automaticamente se não tiver um 6 se for outro numero ele abre uma aba de opções e o jogador pode escolher usar o 6 ou este outro numero para jogar
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "Salvar_Jogadas.txt"; // Camnhinho para o arquivo

            Console.WriteLine("Escolha a Quantidade de jogadores (2-4):"); 
            int quantidadeDeJogadores = int.Parse(Console.ReadLine());

            while (quantidadeDeJogadores < 2 || quantidadeDeJogadores > 4) // Verficação se o jogador escolheu o numero certo de jogadores
            {
                Console.WriteLine("Número inválido. Escolha uma quantidade de jogadores entre 2 e 4:");
                quantidadeDeJogadores = int.Parse(Console.ReadLine());
            }

            Jogadores[] jogadores = new Jogadores[quantidadeDeJogadores]; // Instancia da classe Jogadores como um vetor com a quantidade de casas dada pela quantidade de jogadores
            // Dependendo do numero de jogadores salva a cor 
            if (quantidadeDeJogadores >= 2)
            {
                jogadores[0] = new Jogadores("Vermelho");
                jogadores[1] = new Jogadores("Verde");
            }

            if (quantidadeDeJogadores >= 3)
            {
                jogadores[2] = new Jogadores("Amarelo");
            }

            if (quantidadeDeJogadores == 4)
            {
                jogadores[3] = new Jogadores("Azul");
            }

            Tabuleiro tabuleiro = new Tabuleiro(); // Istancia do tabuleiro que parece inutil (Provavelmente e)

            int jogadorAtual = 0; // Começa no jogador 0 que seria o vermelho
            bool jogoAtivo = true; // Inicia o while e o jogo

            while (jogoAtivo)
            {
                Console.Clear(); // limpa a tela depois de cada jogada
                Jogadores jogador = jogadores[jogadorAtual]; // pega os dados do jogador atual  na classe atraves do vetor jogadores[0]
                Console.WriteLine($"Vez do jogador {jogador.Cor}"); // Pega a cor

                bool jogarNovamente = true; // inicia o while das jogadas

                int contagemDeJogadas = 0;
                int ContagemDeDados = 0;

                int[] SalvarDadosDoJogador = new int[30];
                

                while (jogarNovamente && contagemDeJogadas < 3)
                {
                    int resultadoDado;
                    if (TodosPeoesNaBase(jogador) == true)
                    {
                        resultadoDado = tabuleiro.DadosViciados();
                        SalvarDadosDoJogador[ContagemDeDados] = resultadoDado;
                        contagemDeJogadas++;
                        ContagemDeDados++;
                    }
                    else
                    {
                        resultadoDado = tabuleiro.Dados();
                        SalvarDadosDoJogador[ContagemDeDados] = resultadoDado;
                        contagemDeJogadas++;
                        ContagemDeDados++;
                    }

                    if (contagemDeJogadas > 2)
                        resultadoDado = 3;

                    int c6 = 0;

                    for (int i = 0; i < SalvarDadosDoJogador.Length; i++)
                    {
                        if (SalvarDadosDoJogador[i] == 6)
                        {
                            c6++;
                        }
                    }

                    if (contagemDeJogadas >= 3 && resultadoDado == 6 )
                    {
                        Console.WriteLine($"Resultado do dado: {resultadoDado}");
                        jogarNovamente = false;
                    }

                    else
                    {
                        
                            MostrarDadosParaJogar(SalvarDadosDoJogador);
                            Console.WriteLine($"Escolha um dado para jogar");
                            int EscolherDado = int.Parse(Console.ReadLine());
                            resultadoDado = SalvarDadosDoJogador[EscolherDado - 1];

                            SalvarJogada(filePath, jogador.Cor, resultadoDado); // Salva o resuldado dos dados junto a cor do jogador no arquivo
                        
                       
                        if (TodosPeoesNaBase(jogador) && resultadoDado != 6) // Verificação se todos os peões estão na base e não tirou 6 se tiverem vai pular a vez e o metodo TodosPeoesNaBase(jogador), esta passando o jogador atual permitindo que o metodo acesso o class Jogadores para puxar os dados dos peões  
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

                            MostrarPeoes(jogador); // Pega os dados fo Jogador atual no class Jogadores 
                            Console.WriteLine("Digite 5 para pular sua vez.");

                            string PrimeiraVerificação = Console.ReadLine();
                            while (VerificarPeao(PrimeiraVerificação) == false)
                            {
                                Console.WriteLine("Peão inválido. Escolha novamente ou digite 5 para pular sua vez:");
                                PrimeiraVerificação = Console.ReadLine();
                            }

                            int peaoEscolhido = int.Parse(PrimeiraVerificação);

                            if (peaoEscolhido == 5) // Pula a vez fiz isso a um tempo porque esta dando um bug de loop infinito não sei se eu tirar agora vai resolver mais proferi deixar
                            {
                                Console.WriteLine("Você escolheu pular sua vez.");
                                jogarNovamente = false;
                                break;
                            }

                            while (peaoEscolhido < 1 || peaoEscolhido > 4 || (resultadoDado != 6 && jogador[peaoEscolhido - 1] == -1) || jogador.PeaoIndisponivel(peaoEscolhido - 1)) // Verifica se os peões do jogador se o que ele escolhei esta disponivel para ser jogado ou seja se ele coloca um numero diferente dos peões ou não tirar 6 e selecionar um peão na base alem de peões que ja ganharam o jogo
                            {
                                Console.WriteLine("Peão inválido. Escolha novamente ou digite 5 para pular sua vez:");

                                PrimeiraVerificação = Console.ReadLine();
                                while (VerificarPeao(PrimeiraVerificação) == false)
                                {
                                    Console.WriteLine("Peão inválido. Escolha novamente ou digite 5 para pular sua vez:");
                                    PrimeiraVerificação = Console.ReadLine();
                                }


                                peaoEscolhido = int.Parse(PrimeiraVerificação);

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

                            SalvarJogada(filePath, jogador.Cor, resultadoDado, peaoEscolhido); // Salva a Jogada com o peão escolhido no arquivo

                            if (resultadoDado == 6 && jogador[peaoEscolhido - 1] == -1) // Se o  resultado do dado e 6 e o peão escolhido esta na base entra
                            {
                                jogador[peaoEscolhido - 1] = 0; // move o peão escolhido para a casa 0 que seria a partida
                            }
                            else
                            {
                                int novaPosicao = jogador[peaoEscolhido - 1] + resultadoDado; // Salva temporariamente a nova posição do peão para fazer as verificações 

                                if (jogador.EstaNoCaminhoDaVitoria(jogador[peaoEscolhido - 1])) // Olha se o peão esta no camnhinho da vitoria 
                                {
                                    int posicaoNoCaminho = jogador.PosicaoNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]); // Traz a posição no camhi da vitoria sendo 1,2,3,4,5 ou 6
                                    int novaPosicaoNoCaminho = posicaoNoCaminho + resultadoDado;

                                    if (novaPosicaoNoCaminho == 6) // Verifica se a nova posição no caminho da vitoria e a 6 se for esse peão fica indisponivel e o jogador pode jogar de novo
                                    {
                                        jogador.TornarPeaoIndisponivel(peaoEscolhido - 1);
                                        Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} chegou ao final e está indisponível.");
                                        jogarNovamente = true;
                                    }

                                    else if (novaPosicaoNoCaminho <= 6) // se não for a 6 não permite o jogador usar o peão e pula a vez
                                    {
                                        jogarNovamente = false;
                                        Console.WriteLine("Movimento inválido. Escolha outra peça.");

                                    }

                                }
                                else
                                {
                                    if (novaPosicao <= 52) // se a nova posição não for o camnhinho da vitoria
                                    {
                                        jogador[peaoEscolhido - 1] = novaPosicao; // Muda a posição do jogador permamente
                                    }
                                    else
                                    {
                                        int posicaoNoCaminhoDaVitoria = novaPosicao - 52; // Salva temporariamente a posição no camnhicda vitoria 
                                        if (posicaoNoCaminhoDaVitoria <= 6) // se a posição no caminho for menor = a 6
                                        {
                                            jogador[peaoEscolhido - 1] = 52 + posicaoNoCaminhoDaVitoria; // salvar 
                                        }
                                        else
                                        {
                                            Console.WriteLine("Movimento inválido. Escolha outra peça.");
                                            jogarNovamente = false;
                                            continue;
                                        }
                                    }


                                    ComerPeaoInimigo(jogador, peaoEscolhido - 1, jogadores);
                                    jogarNovamente = resultadoDado == 6;
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
                }

                if (jogoAtivo)
                {
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para passar a vez...");
                    Console.ReadLine();

                    jogadorAtual = (jogadorAtual + 1) % quantidadeDeJogadores;
                }


            }

            Console.ReadLine();
        }

        static bool VerificarPeao(string peao )
        {
            if (peao == "1" || peao == "2" || peao == "3" || peao == "4" || peao == "5")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void MostrarDadosParaJogar(int[] Dados)
        {
            for(int i = 0; i < Dados.Length;i++)
            {
                if(Dados[i] != 0)
                Console.WriteLine($"Dado {i + 1} Valor: {Dados[i]}");
            }
        }
        static void SalvarJogada(string filePath, string corJogador, int resultadoDado, int? peaoEscolhido = null)
        {
            using (StreamWriter arq = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                if (peaoEscolhido.HasValue)
                {
                    arq.WriteLine($"Jogador: {corJogador}, Dado: {resultadoDado}, Peão Escolhido: {peaoEscolhido}");
                }
                else
                {
                    arq.WriteLine($"Jogador: {corJogador}, Dado: {resultadoDado}");
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

        static void ComerPeaoInimigo(Jogadores jogador, int peaoIndex, Jogadores[] jogadores)
        {
            int posicaoPeao = jogador[peaoIndex];
            foreach (var jogadorInimigo in jogadores)
            {
                if (jogadorInimigo != jogador)
                {
                    for (int i = 0; i < jogadorInimigo.Peoes.Length; i++)
                    {
                        if (jogadorInimigo[i] == posicaoPeao && jogadorInimigo[i] != 0 && jogadorInimigo[i] != 10 && jogadorInimigo[i] != 49 && jogadorInimigo[i] != 29 && jogadorInimigo[i] != 36)
                        {
                            jogadorInimigo[i] = -1;
                            Console.WriteLine($"Peão do jogador {jogadorInimigo.Cor} foi comido e voltou para a base.");
                        }
                    }
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
                return r.Next(6,7);
            }

            public int DadosViciados()
            {
                return r.Next(5,7);
            }


        }
    }
}
