using System;
using System.Text;
using System.IO;

namespace Ludo
{
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
            int ContagemdeSeis = 0; // Contagem de numeros seis 
            int jogadorAtual = 0; // Começa no jogador 0 que seria o vermelho
            bool jogoAtivo = true; // Inicia o while e o jogo

            while (jogoAtivo)
            {
                Console.Clear(); // limpa a tela depois de cada jogada
                Jogadores jogador = jogadores[jogadorAtual]; // pega os dados do jogador atual  na classe atraves do vetor jogadores[0]
                Console.WriteLine($"Vez do jogador {jogador.Cor}"); // Pega a cor

                bool jogarNovamente = true; // inicia o while das jogadas
                int contagemDeJogadas = 0;

                while (jogarNovamente && contagemDeJogadas < 3) // se o numero de jogadas passar de 3 pula a vez
                {
                    int resultadoDado;
                    int contagemSeis = 0; // contador para 6 consecutivos

                    if (TodosPeoesNaBase(jogador))
                    {
                        resultadoDado = tabuleiro.DadosViciados();
                        contagemDeJogadas++;
                        Console.WriteLine($"Resultado do dado {contagemDeJogadas}: {resultadoDado}");
                    }
                    else
                    {
                        resultadoDado = tabuleiro.Dados();
                        contagemDeJogadas++;
                        Console.WriteLine($"Resultado do dado {contagemDeJogadas}: {resultadoDado}");
                    }

                    while (resultadoDado == 6) // se o 1 dado for 6 ele entra aqui e roda os dados de nono se cair outro 6 roda outra vez e se passar de 3 pula a vez
                    {
                        contagemSeis++;
                        if (contagemSeis == 3)
                        {
                            jogarNovamente = false;
                            break;
                        }

                        resultadoDado = tabuleiro.Dados();
                        contagemDeJogadas++;
                        Console.WriteLine($"Resultado do dado {contagemDeJogadas}: {resultadoDado}");

                        if (contagemDeJogadas >= 3)
                        {
                            break;
                        }
                    }

                    if (contagemSeis < 3) // Se a quantidade se seis for menor que 3 entra 
                    {
                        {
                            if (contagemDeJogadas == 1) // Caso o dado so tenha rodado 1 vez
                            {
                                Console.WriteLine($"Você tirou: {resultadoDado}");
                            }



                            else
                            {
                                // Se o dado rodou mais de uma vez aparece para o jogador escolher a opção 
                                
                                    Console.WriteLine("Escolha Qual voce quer jogar");
                                    Console.WriteLine("1-Jogar o dado com numero 6");
                                    Console.WriteLine($"2-Jogar o dado com numero {resultadoDado}");
                                    if (TodosPeoesNaBase(jogador) == true) // Se ele tiver todos os peãos na base joga o 6 automaticamnetne
                                    {
                                        Console.WriteLine("Todos os peões estão na base escolhendo automaticamente o numero 6");
                                        resultadoDado = 6;
                                    }
                                    else
                                    {
                                        int escolherDado = int.Parse(Console.ReadLine()); // se ele escolher o dado 2 não muda nada mais se ele escolher o 1 ele muda o resultado para 6
                                        if (escolherDado == 1)
                                            resultadoDado = 6;
                                    }

                                    ContagemdeSeis++;
                                
                                Console.WriteLine($"Você escolheu  : {resultadoDado}");
                            }



                            SalvarJogada(filePath, jogador.Cor, resultadoDado); // Salva o resuldado dos dados junto a cor do jogador no arquivo


                            if (TodosPeoesNaBase(jogador) && resultadoDado != 6) // Verificação se todos os peões estão na base e não tirou 6 se tiverem vai pular a vez e o metodo TodosPeoesNaBase(jogador), esta passando o jogador atual permitindo que o metodo acesso o class Jogadores para puxar os dados dos peões  
                            {
                                Console.WriteLine("Todos os peões estão na base e você não tirou 6. Sua vez foi pulada.");
                                jogarNovamente = false;
                            }
                            else
                            {

                                Console.WriteLine("Escolha um peão para mover.");


                                MostrarPeoes(jogador); // Mostra os peções passando como parametro os dados do jogador na claase Jogador
                                Console.WriteLine("Digite 5 para pular sua vez.");

                                string PrimeiraVerificação = Console.ReadLine(); // verificação se o jogador colocou um numeor valido
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


                                        ComerPeaoInimigo(jogador, peaoEscolhido - 1, jogadores); // metodo para comer o peão o peão iminigo passando o jogador atual e todos os outros jogadores alem do peão escolhido pelo jogador
                                        jogarNovamente = resultadoDado == 6;
                                    }
                                }

                                Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} está na posição {jogador[peaoEscolhido - 1]}");

                                if (jogador.EstaNoCaminhoDaVitoria(jogador[peaoEscolhido - 1])) // Para saber se o peão esta 
                                {
                                    int posicaoNoCaminho = jogador.PosicaoNoCaminhoDaVitoria(jogador[peaoEscolhido - 1]); // Mudar a pos~ição no caminho da vitoria para 
                                    Console.WriteLine($"Peão {peaoEscolhido} do jogador {jogador.Cor} está no caminho da vitória, na casa {posicaoNoCaminho}");

                                    if (posicaoNoCaminho == 6) // se o jogador estiver na posição 6 do camhinho ele ganha 
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

                        jogadorAtual = (jogadorAtual + 1) % quantidadeDeJogadores; // pula vez do jogador
                    }


                }

                Console.ReadLine();
            }

            static bool VerificarPeao(string peao)
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

         
            static void SalvarJogada(string filePath, string corJogador, int resultadoDado, int? peaoEscolhido = null) // Salva no arquivo 
            {
                using (StreamWriter arq = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    if (peaoEscolhido.HasValue) // se o peão escolhido tiver algum valor nele entra aqui 
                    {
                        arq.WriteLine($"Jogador: {corJogador}, Dado: {resultadoDado}, Peão Escolhido: {peaoEscolhido}");
                    }
                    else // se não aqui , fiz isso pq no começo do jogo quando todos os peão estao na base e ele não consegue mexer com nenhum deles o peão escolhidido da um bug no arquivo
                    {
                        arq.WriteLine($"Jogador: {corJogador}, Dado: {resultadoDado}");
                    }
                }
            }

            static bool TodosPeoesNaBase(Jogadores jogador) // Ve se todos os peões estãi na base 
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
                foreach (var jogadorInimigo in jogadores) // o var e usado para criar uma variacel que se identifica com quem esta dando o valor a ela ai esta dando o vetor int jogadores então ele e um vetor int
                { // passa os velores de jogadores para o de jogadoresinimigos
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
        }

        class Jogadores
        {
            private string cor;
            private int[] peoes = { -1, -1, -1, -1 }; // peões dos jogadores nas bases
            private bool[] peoesIndisponiveis = { false, false, false, false }; // Para determiniar a vitoria todos eles tem que estar true

            public Jogadores(string cor) // Instancia jogadores pedindo a cors
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

            public bool EstaNoCaminhoDaVitoria(int posicao) // Metodo que confere se um peão esta ou não no camnhinho da vitoria 
            {
                return posicao > 52 && posicao <= 58;
            }

            public int PosicaoNoCaminhoDaVitoria(int posicao) // Conta para passar o numero certo no camhinho da vitoria 
            {
                return posicao - 52;
            }

            public void TornarPeaoIndisponivel(int peao) // Muda o peão indisponivel para true 
            {
                peoesIndisponiveis[peao] = true;
            }

            public bool PeaoIndisponivel(int peao) // Conta para saber se todos os peões estão indisponoveis ou não 
            {
                return peoesIndisponiveis[peao];
            }

            public bool GanhouOJogo() // Se todos os peões estiverem indisponiveis o jogador ganha o jogo 
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
                return r.Next(1, 7);
            }

            public int DadosViciados()
            {
                return r.Next(5, 7);
            }


        }
    }
}
