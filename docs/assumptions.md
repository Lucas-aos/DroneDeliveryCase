# Premissas do Projeto

Este documento registra as decisões adotadas para pontos que não estão completamente definidos no enunciado.

As premissas poderão ser revisadas caso a validação do algoritmo revele inconsistências ou novos cenários não considerados.

---

## Premissas definidas

### Composição de uma viagem

**Decisão**

Uma viagem poderá atender vários pedidos e visitar vários destinos.

**Justificativa**

O objetivo do desafio é reduzir a quantidade de viagens. Permitir o agrupamento de pedidos está alinhado com esse objetivo.

---

### Fórmula de cálculo da distância

**Decisão**

Será utilizada a distância euclidiana entre dois pontos.

**Justificativa**

As coordenadas fornecidas representam posições em um plano cartesiano e drones podem ser modelados como deslocando-se em linha reta no MVP.

---

### Localização da base

**Decisão**

Todos os drones partirão de uma única base localizada na coordenada `(0,0)`.

**Justificativa**

O enunciado não define uma origem. Utilizar `(0,0)` fornece uma referência única para todos os cálculos.

---

### Retorno do drone à base

**Decisão**

Toda viagem deverá terminar com o retorno do drone à base.

**Justificativa**

O alcance máximo passa a representar o percurso completo da missão e permite reutilizar o drone em viagens posteriores.

---

### Distância total de uma viagem

**Decisão**

A distância total será calculada pela soma de todos os trechos percorridos desde a saída da base até o retorno.

---

### Nível de otimização do MVP

**Decisão**

O MVP utilizará uma estratégia heurística simples para reduzir o número de viagens, sem garantir uma solução ótima.

**Justificativa**

Essa abordagem mantém a implementação compatível com um projeto de estágio.

---

### Realização de várias viagens

**Decisão**

Um mesmo drone poderá realizar várias viagens durante o planejamento e a simulação.

A política completa de seleção e reutilização está descrita na seção “Política de seleção e reutilização dos drones”.

**Justificativa**

O enunciado não estabelece limite de viagens e o MVP não considera indisponibilidade provocada por recarga, desgaste ou tempo de operação.

---

### Pedidos impossíveis de atender

**Decisão**

Pedidos impossíveis serão identificados e apresentados separadamente, sem impedir o processamento dos demais pedidos.

**Justificativa**

Um pedido inviável não deve comprometer toda a simulação.

---

### Utilização da prioridade

**Decisão**

A prioridade será considerada durante o planejamento das entregas.

Pedidos com prioridades diferentes poderão ser agrupados na mesma viagem, desde que as restrições de peso e alcance sejam respeitadas.

A prioridade também será utilizada nos critérios de desempate definidos para a seleção de pedidos e para a ordenação dos destinos.

**Justificativa**

Essa abordagem mantém a urgência como um critério relevante sem impedir o aproveitamento da capacidade dos drones ou aumentar desnecessariamente o número de viagens.

---

### Escolha do primeiro pedido da viagem

**Decisão**

Os pedidos serão inicialmente considerados em ordem de prioridade.

Cada nova viagem será iniciada pelo pedido de maior peso dentro da maior prioridade ainda disponível.

**Justificativa**

A prioridade permanece como o critério principal do planejamento, enquanto o peso é utilizado como critério secundário para tratar primeiro os pedidos mais difíceis de acomodar na capacidade dos drones.

**Pendência**

Ainda deve ser definido o critério de desempate quando dois ou mais pedidos atendíveis possuírem a mesma prioridade e o mesmo peso.

---

### Agrupamento de pedidos em uma viagem

**Decisão**

Depois da escolha do primeiro pedido, novos pedidos poderão ser incluídos enquanto a capacidade de peso e o alcance máximo do drone forem respeitados.

Para cada pedido candidato, sua inclusão será simulada temporariamente. A rota do conjunto resultante será recalculada utilizando a heurística do vizinho mais próximo.

Entre os candidatos válidos, será escolhido aquele que provocar o menor aumento na distância total da viagem.

Um pedido considerado inválido durante a formação de uma viagem continuará pendente e poderá ser avaliado novamente em outra viagem ou com outro drone.

**Critérios de desempate**

Em caso de empate no aumento da distância, será utilizada a seguinte ordem:

1. maior prioridade
2. maior peso
3. menor ordem de entrada
4. menor identificador

**Justificativa**

A avaliação utiliza a mesma estratégia de roteamento que será adotada pela viagem, evitando selecionar um pedido com base em uma rota diferente daquela utilizada para validar o alcance.

---

### Ordem de visita dos destinos

**Decisão**

A ordem de visita dos destinos será definida pela heurística do vizinho mais próximo.

A rota será iniciada na base. Em cada etapa, será escolhido o destino ainda não visitado mais próximo da posição atual. Depois que todos os destinos forem atendidos, o drone retornará à base.

**Justificativa**

A heurística é simples de implementar, compreender e testar. Embora não garanta a menor rota possível, tende a produzir rotas razoáveis sem exigir a avaliação de todas as combinações de destinos.

---

### Momento de recálculo da rota

**Decisão**

A rota será simulada novamente durante a avaliação de cada pedido candidato.

Depois que o candidato vencedor for escolhido, a rota simulada correspondente passará a ser a rota oficial da viagem.

**Justificativa**

Essa abordagem mantém a seleção dos pedidos, o cálculo da distância e a validação do alcance baseados na mesma heurística.

Apesar de executar o cálculo do vizinho mais próximo várias vezes, sua complexidade é aceitável para o volume esperado no MVP.

---

### Encerramento de uma viagem

**Decisão**

A viagem será encerrada quando:

- não houver mais pedidos disponíveis; ou
- nenhum pedido restante puder ser incluído sem ultrapassar a capacidade de peso ou o alcance máximo do drone, considerando o retorno à base.

**Justificativa**

A viagem continua recebendo pedidos enquanto existir uma inclusão válida, sem depender de percentuais arbitrários de ocupação ou limites artificiais de aumento da distância.

---

### Limitações da estratégia de planejamento

A estratégia adotada é heurística e não oferece garantia matemática de solução ótima.

Em particular:

- o vizinho mais próximo não garante a menor rota possível;
- a escolha sucessiva do menor aumento de distância não garante o menor número global de viagens;
- a qualidade do resultado pode depender da ordem inicial dos pedidos e dos critérios de desempate.

Essas limitações são consideradas aceitáveis para o MVP, cujo objetivo é apresentar uma solução simples, previsível e defensável.

---

### Desempate na heurística do vizinho mais próximo

**Decisão**

Quando dois ou mais destinos não visitados estiverem à mesma distância da posição atual do drone, será utilizada a seguinte ordem de desempate:

1. maior prioridade;
2. menor ordem de entrada;
3. maior peso;
4. menor identificador.

**Justificativa**

A prioridade preserva a urgência dos pedidos quando não existe diferença logística entre os destinos.

A ordem de entrada evita que pedidos mais antigos sejam ultrapassados sem necessidade e mantém o comportamento previsível.

O peso é utilizado como critério secundário para preservar a consistência com a estratégia geral de tratar primeiro os pacotes mais difíceis de acomodar. Embora ele não altere diretamente a distância da rota, sua utilização ocorre apenas depois dos critérios de prioridade e ordem de entrada.

O identificador é utilizado como último critério para garantir que empates completos sempre produzam o mesmo resultado.

---

## Política de seleção e reutilização dos drones

### Decisão

Antes de iniciar cada nova viagem, o planejamento identifica os drones capazes de atender individualmente pelo menos um pedido ainda pendente, respeitando simultaneamente:

- a capacidade máxima de peso;
- o alcance necessário para sair da base, chegar ao destino do pedido e retornar à base.

Entre os drones elegíveis, será escolhido aquele com maior capacidade de peso.

Em caso de empate, serão utilizados, nesta ordem:

1. maior alcance;
2. menor ordem de entrada;
3. menor identificador, caso aplicável.

A ordem de entrada dos drones será estável e única durante o planejamento.

A seleção será repetida antes de cada nova viagem. Todos os drones elegíveis voltam a ser considerados, permitindo que um mesmo drone seja escolhido em viagens consecutivas e reutilizado sem limite.

Não haverá rodízio, balanceamento, reserva de drones ou preferência por drones ainda não utilizados.

### Justificativa

A priorização do drone com maior capacidade de peso aumenta o potencial de agrupamento de pedidos e está alinhada ao objetivo principal de reduzir a quantidade de viagens.

A estratégia permanece simples, determinística, testável e adequada ao escopo do MVP.

### Limitação conhecida

A política é heurística e não garante o menor número global de viagens. Em especial, um drone com maior capacidade de peso pode ter alcance inferior ao de outro drone e produzir um planejamento menos eficiente em determinados cenários.

---
