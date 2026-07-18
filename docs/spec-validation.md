# Validação da Especificação Funcional

## Objetivo

Este documento registra a validação manual da especificação funcional antes da implementação.

O objetivo é verificar se todas as regras documentadas produzem um comportamento determinístico e suficiente para implementação do algoritmo.

Cada cenário é executado manualmente, seguindo rigorosamente as regras definidas em `assumptions.md`.

---

## Cenário 1 — Fluxo básico

### Objetivo

Validar o fluxo principal do algoritmo sem empates ou situações especiais.

### Dados de entrada

#### Drones

|  Drone  | Capacidade | Alcance |
|---------|------------|---------|
|   D1    |    10 kg   |  30 km  |
|   D2    |    8 kg    |  25 km  |
|   D3    |    12 kg   |  35 km  |

#### Pedidos

| Pedido | Prioridade | Peso | Coordenadas |
|--------|------------|------|-------------|
|   P1   |    Alta    | 5 kg |    (2,2)    |
|   P2   |    Média   | 2 kg |    (5,1)    |
|   P3   |    Baixa   | 2 kg |    (6,4)    |

### Execução

#### Etapa 1 — Identificação de pedidos impossíveis

Foram avaliados todos os pedidos individualmente, verificando se cada um pode ser atendido por pelo menos um drone da frota, considerando simultaneamente:

- peso do pedido em relação à capacidade do drone
- alcance necessário para sair da base, realizar a entrega e retornar

##### Pedido P1

Peso do pedido:

- 5 kg

Verificação de capacidade:

- D1: 5 kg ≤ 10 kg
- D2: 5 kg ≤ 8 kg
- D3: 5 kg ≤ 12 kg

Todos os drones possuem capacidade suficiente para atender P1 individualmente.

Verificação de alcance:

- Distância Base → P1:
  - √(2² + 2²) = 2,83 km
- Distância P1 → Base:
  - √(2² + 2²) = 2,83 km
- Alcance necessário:
  - 2,83 + 2,83 = 5,66 km

Comparação:

- D1: 5,66 km ≤ 30 km
- D2: 5,66 km ≤ 25 km
- D3: 5,66 km ≤ 35 km

Todos os drones possuem alcance suficiente para atender P1 individualmente.

Resultado:

P1 é possível.

---

##### Pedido P2

Peso do pedido:

- 2 kg

Verificação de capacidade:

- D1: 2 kg ≤ 10 kg
- D2: 2 kg ≤ 8 kg
- D3: 2 kg ≤ 12 kg

Todos os drones possuem capacidade suficiente para atender P2 individualmente.

Verificação de alcance:

- Distância Base → P2:
  - √(5² + 1²) = 5,10 km
- Distância P2 → Base:
  - √(5² + 1²) = 5,10 km
- Alcance necessário:
  - 5,10 + 5,10 = 10,20 km

Comparação:

- D1: 10,20 km ≤ 30 km
- D2: 10,20 km ≤ 25 km
- D3: 10,20 km ≤ 35 km

Todos os drones possuem alcance suficiente para atender P2 individualmente.

Resultado:

P2 é possível.

---

##### Pedido P3

Peso do pedido:

- 2 kg

Verificação de capacidade:

- D1: 2 kg ≤ 10 kg
- D2: 2 kg ≤ 8 kg
- D3: 2 kg ≤ 12 kg

Todos os drones possuem capacidade suficiente para atender P3 individualmente.

Verificação de alcance:

- Distância Base → P3:
  - √(6² + 4²) = 7,21 km
- Distância P3 → Base:
  - √(6² + 4²) = 7,21 km
- Alcance necessário:
  - 7,21 + 7,21 = 14,42 km

Comparação:

- D1: 14,42 km ≤ 30 km
- D2: 14,42 km ≤ 25 km
- D3: 14,42 km ≤ 35 km

Todos os drones possuem alcance suficiente para atender P3 individualmente.

Resultado:

P3 é possível.

---

Conclusão da etapa:

Todos os pedidos podem ser atendidos individualmente por pelo menos um drone da frota.

Nenhum pedido foi classificado como impossível.


---

#### Etapa 2 — Seleção do drone

Foram avaliados os drones capazes de atender individualmente pelo menos um pedido ainda pendente.

##### Drone D1

- Capacidade: 10 kg
- Alcance: 30 km
- Atende individualmente P1, P2 e P3
- Situação: elegível

##### Drone D2

- Capacidade: 8 kg
- Alcance: 25 km
- Atende individualmente P1, P2 e P3
- Situação: elegível

##### Drone D3

- Capacidade: 12 kg
- Alcance: 35 km
- Atende individualmente P1, P2 e P3
- Situação: elegível

Aplicação da política de seleção:

1. Maior capacidade de peso

- D1: 10 kg
- D2: 8 kg
- D3: 12 kg

D3 possui a maior capacidade de peso.

Não foi necessário aplicar os critérios de desempate por alcance, ordem de entrada ou identificador.

Resultado:

Drone selecionado: D3.

Justificativa:

D3 é elegível e possui a maior capacidade de peso entre os drones disponíveis.

---

#### Etapa 3 — Escolha do primeiro pedido

Foram considerados os pedidos pendentes que podem ser atendidos individualmente pelo drone D3:

- P1 — prioridade Alta, peso 5 kg
- P2 — prioridade Média, peso 2 kg
- P3 — prioridade Baixa, peso 2 kg

Aplicação dos critérios:

1. Maior prioridade disponível

- P1 possui prioridade Alta
- P2 possui prioridade Média
- P3 possui prioridade Baixa

Como P1 é o único pedido de prioridade Alta, não foi necessário aplicar os critérios seguintes de peso, ordem de entrada ou identificador.

**Pedido selecionado**

P1

**Justificativa**

P1 possui a maior prioridade entre os pedidos pendentes atendíveis pelo drone D3.

---

#### Etapa 4 — Formação inicial da viagem

A viagem foi iniciada com o pedido P1.

Pedidos incluídos:

- P1

Peso acumulado:

- 5 kg

Capacidade do drone:

- 12 kg

Validação da capacidade:

- 5 kg ≤ 12 kg
- Resultado: aprovado

Rota inicial:

Base → P1 → Base

Cálculo da distância:

- Base → P1:
  - √((2 - 0)² + (2 - 0)²) = 2,83 km
- P1 → Base:
  - √((0 - 2)² + (0 - 2)²) = 2,83 km

Distância total:

- 2,83 + 2,83 = 5,66 km

Validação do alcance:

- 5,66 km ≤ 35 km
- Resultado: aprovado

**Estado inicial da viagem**

- Drone: D3
- Pedidos: P1
- Peso acumulado: 5 kg
- Rota: Base → P1 → Base
- Distância: 5,66 km

---

#### Etapa 5 — Avaliação dos candidatos

Após a inclusão de P1, permaneceram pendentes:

- P2
- P3

Cada pedido pendente foi avaliado individualmente.

Para cada candidato:

1. o pedido foi incluído temporariamente na viagem;
2. a rota foi recalculada utilizando a heurística do Vizinho Mais Próximo;
3. foram verificadas as restrições de capacidade e alcance;
4. foi calculado o aumento da distância total da viagem.

##### Avaliação do candidato P2

Simulação da inclusão do candidato:

Conjunto temporário de pedidos:

- P1
- P2

Peso acumulado:

- 5 kg + 2 kg = 7 kg

Validação da capacidade:

- 7 kg ≤ 12 kg
- Resultado: aprovado

Aplicação da heurística do Vizinho Mais Próximo

A partir da base:

- Distância até P1: 2,83 km
- Distância até P2: 5,10 km

P1 é o destino mais próximo da base.

A partir de P1:

- Distância até P2: 3,16 km

Como P2 é o único pedido restante, ele é selecionado.

Rota obtida:

Base → P1 → P2 → Base

Cálculo da distância total:

- Base → P1:
  - √((2 − 0)² + (2 − 0)²) = 2,83 km
- P1 → P2:
  - √((5 − 2)² + (1 − 2)²)
  - √(3² + (-1)²)
  - √10 = 3,16 km
- P2 → Base:
  - √((0 − 5)² + (0 − 1)²)
  - √26 = 5,10 km

Distância total da rota:

- 2,83 + 3,16 + 5,10 = 11,09 km

Validação do alcance:

- 11,09 km ≤ 35 km
- Resultado: aprovado

Aumento da distância em relação à rota atual:

- 11,09 − 5,66 = 5,43 km

Resultado:

P2 é um candidato válido.

---

##### Avaliação do candidato P3

Simulação da inclusão do candidato:

Conjunto temporário de pedidos:

- P1
- P3

Peso acumulado:

- 5 kg + 2 kg = 7 kg

Validação da capacidade:

- 7 kg ≤ 12 kg
- Resultado: aprovado

Aplicação da heurística do Vizinho Mais Próximo

A partir da base:

- Distância até P1: 2,83 km
- Distância até P3: 7,21 km

P1 é o destino mais próximo da base.

A partir de P1:

- Distância até P3: 4,47 km

Como P3 é o único pedido restante, ele é selecionado.

Rota obtida:

Base → P1 → P3 → Base

Cálculo da distância total:

- Base → P1:
  - √((2 − 0)² + (2 − 0)²) = 2,83 km
- P1 → P3:
  - √((6 − 2)² + (4 − 2)²)
  - √(4² + 2²)
  - √20 = 4,47 km
- P3 → Base:
  - √((0 − 6)² + (0 − 4)²)
  - √52 = 7,21 km

Distância total da rota:

- 2,83 + 4,47 + 7,21 = 14,51 km

Validação do alcance:

- 14,51 km ≤ 35 km
- Resultado: aprovado

Aumento da distância em relação à rota atual:

- 14,51 − 5,66 = 8,85 km

Resultado:

P3 é um candidato válido.

---

#### Etapa 6 — Escolha do melhor candidato

Os dois candidatos respeitam as restrições de capacidade e alcance.

Comparação dos aumentos de distância:

| Candidato | Peso acumulado | Distância da rota | Aumento de distância |
|-----------|----------------|-------------------|----------------------|
|    P2     |      7 kg      |      11,09 km     |        5,43 km       |
|    P3     |      7 kg      |      14,51 km     |        8,85 km       |

Aplicação da política de seleção:

1. Menor aumento de distância

P2 apresenta o menor aumento de distância.

Como apenas um candidato apresentou o menor aumento de distância, não foi necessário aplicar os critérios de desempate por prioridade, peso, ordem de entrada ou identificador.

Resultado:

P2 foi incorporado à viagem.

A rota simulada torna-se a rota oficial:

Base → P1 → P2 → Base

---

#### Etapa 7 — Continuação e encerramento da viagem

Após a inclusão de P2, o pedido P3 permanece pendente e deve ser avaliado novamente.

##### Avaliação de P3

Pedidos considerados:

- P1
- P2
- P3

Peso acumulado:

- 5 kg + 2 kg + 2 kg = 9 kg

Validação da capacidade:

- 9 kg ≤ 12 kg
- Resultado: aprovado

Aplicação da heurística do Vizinho Mais Próximo:

A partir da base:

- Distância até P1: 2,83 km
- Distância até P2: 5,10 km
- Distância até P3: 7,21 km

P1 é o destino mais próximo.

A partir de P1:

- Distância até P2: 3,16 km
- Distância até P3: 4,47 km

P2 é o destino mais próximo.

A partir de P2, o único destino restante é P3.

Rota recalculada:

Base → P1 → P2 → P3 → Base

Cálculo da distância total:

- Base → P1: 2,83 km
- P1 → P2: 3,16 km
- P2 → P3:
  - √((6 - 5)² + (4 - 1)²)
  - √(1² + 3²)
  - √10 = 3,16 km
- P3 → Base: 7,21 km

Distância total da rota:

- 2,83 + 3,16 + 3,16 + 7,21 = 16,36 km

Validação do alcance:

- 16,36 km ≤ 35 km
- Resultado: aprovado

Aumento de distância em relação à rota anterior:

- 16,36 - 11,09 = 5,27 km

**Resultado**

P3 é um candidato válido e foi incluído na viagem.

Como não existem outros candidatos pendentes, P3 é incorporado à viagem.

A rota simulada passa a ser a rota oficial:

Base → P1 → P2 → P3 → Base

Estado final da viagem:

- Drone: D3
- Pedidos: P1, P2 e P3
- Peso acumulado: 9 kg
- Capacidade do drone: 12 kg
- Distância total: 16,36 km
- Alcance do drone: 35 km

Após a inclusão de P3, não existem outros pedidos pendentes.

**Motivo do encerramento**

Todos os pedidos possíveis foram incluídos na viagem.

---

## Resumo da execução

|          Item         |          Resultado         |
|-----------------------|----------------------------|
| Drone selecionado     |             D3             |
| Pedidos impossíveis   |           Nenhum           |
| Viagens realizadas    |              1             |
| Rota final            | Base → P1 → P2 → P3 → Base |
| Peso total            |            9 kg            |
| Distância total       |           16,36 km         |
| Pedidos pendentes     |           Nenhum           |

---

### Resultado esperado

O drone D3 deve ser selecionado por possuir a maior capacidade de peso.

A viagem deve começar pelo pedido P1, por possuir a maior prioridade.

Na primeira avaliação de candidatos, P2 deve ser escolhido por produzir um aumento de distância menor que P3.

Em seguida, P3 deve ser incluído, pois o peso acumulado e a distância total permanecem dentro dos limites do drone.

Resultado final esperado:

- uma viagem
- drone D3
- rota: Base → P1 → P2 → P3 → Base
- peso total: 9 kg
- distância total: 16,36 km
- nenhum pedido pendente
- nenhum pedido impossível

### Resultado obtido

A execução manual produziu exatamente o comportamento esperado.

Todas as decisões foram tomadas utilizando as regras documentadas em `assumptions.md`, sem necessidade de interpretações adicionais.

### Conclusão

☑ Aprovado

**Observações**

O cenário validou:

- seleção do drone por maior capacidade
- escolha inicial por prioridade
- formação da viagem
- recálculo completo da rota
- escolha do candidato pelo menor aumento de distância
- validação do peso acumulado
- validação do alcance com retorno à base
- atualização da rota oficial
- encerramento da viagem após o atendimento de todos os pedidos

Não foram exercitados critérios de desempate, pois nenhuma das comparações resultou em empate.

---

## Regras validadas

| Regra | Status |
|--------|--------|
| Identificação de pedidos impossíveis | ✅ |
| Seleção do drone | ✅ |
| Escolha do primeiro pedido | ✅ |
| Formação da viagem | ✅ |
| Avaliação dos candidatos | ✅ |
| Vizinho mais próximo | ✅ |
| Atualização da rota oficial | ✅ |
| Encerramento da viagem | ✅ |

---
