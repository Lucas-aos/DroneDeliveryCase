# Premissas do Projeto

Este documento registra decisões adotadas para pontos que não estão completamente definidos no enunciado.

As questões permanecem nesta seção enquanto estiverem em análise. Depois de decididas, serão registradas como premissas do projeto.

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

**Observação**

A estratégia utilizada para definir a ordem dos destinos ainda será definida.

---

### Nível de otimização do MVP

**Decisão**

O MVP utilizará uma estratégia heurística simples para reduzir o número de viagens, sem garantir uma solução ótima.

**Justificativa**

Essa abordagem mantém a implementação compatível com um projeto de estágio.

---

### Realização de várias viagens

**Decisão**

Um mesmo drone poderá realizar várias viagens durante a simulação.

**Justificativa**

O enunciado não estabelece limite de viagens nem simulação de tempo ou recarga.

---

### Pedidos impossíveis de atender

**Decisão**

Pedidos impossíveis serão identificados e apresentados separadamente, sem impedir o processamento dos demais pedidos.

**Justificativa**

Um pedido inviável não deve comprometer toda a simulação.

---

### Utilização da prioridade

**Decisão**

Os pedidos serão inicialmente considerados em ordem de prioridade (Alta → Média → Baixa). Essa ordenação representa o critério inicial para o planejamento das entregas.

Pedidos com prioridades diferentes poderão ser agrupados na mesma viagem, desde que as demais restrições de peso e alcance sejam respeitadas.

**Justificativa**

Essa abordagem mantém a prioridade como um critério relevante para o planejamento sem impedir o aproveitamento da capacidade dos drones ou aumentar desnecessariamente o número de viagens.

---

## Questões em aberto

### Ordem de visita dos destinos

Embora a distância total da viagem já tenha sido definida, ainda não foi estabelecida a estratégia para ordenar os destinos.

Questões:

- Os destinos serão visitados por proximidade?
- Será utilizada uma estratégia de vizinho mais próximo?
- A prioridade influenciará essa ordem?
- Como serão resolvidos empates?

**Status:** Em análise.