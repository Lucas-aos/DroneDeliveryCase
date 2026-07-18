# Premissas do Projeto

Este documento registra decisões adotadas para pontos que não estão completamente definidos no enunciado.

As questões permanecem nesta seção enquanto estiverem em análise. Depois de decididas, serão registradas como premissas do projeto.

---

## Questões em aberto

### Utilização da prioridade

A forma como a prioridade influenciará o planejamento das entregas ainda não foi definida.

Questões:

- A prioridade define a ordem de processamento dos pedidos?
- Ela será utilizada apenas como critério de desempate?
- Ela participará de uma pontuação de alocação?
- Pedidos com prioridades diferentes poderão ser agrupados na mesma viagem?

**Status:** Em análise.

---

### Fórmula de cálculo da distância

A forma de calcular a distância entre duas coordenadas ainda não foi definida.

Questões:

- Será utilizada distância euclidiana?
- Será utilizada distância Manhattan?
- As coordenadas representarão quilômetros ou unidades abstratas?

**Status:** Em análise.

---

### Localização da base

O ponto de origem dos drones ainda não foi definido.

Questões:

- Todos os drones partirão da coordenada `(0,0)`?
- A base terá coordenadas configuráveis?
- Todos os drones utilizarão a mesma base?

**Status:** Em análise.

---

### Retorno do drone à base

Ainda não foi definido se o alcance da viagem deve incluir o retorno do drone.

Questões:

- O drone deverá retornar à base ao final de toda viagem?
- O alcance considerará somente o trajeto de ida?
- Em uma viagem com vários destinos, o retorno será calculado a partir do último destino?

**Status:** Em análise.

---

---

### Composição de uma viagem

Ainda não foi definido quantos pedidos e destinos podem fazer parte de uma viagem.

Questões:

- Uma viagem pode atender vários pedidos?
- Uma viagem pode visitar vários destinos?
- A ordem de visita fará parte do planejamento?

**Status:** Em análise.

---

### Distância total de uma viagem

Ainda não foi definido como calcular o percurso quando uma viagem contém vários destinos.

Questões:

- A distância será a soma dos trechos percorridos?
- Qual regra definirá a ordem dos destinos?
- Será utilizada uma estratégia simples de proximidade?

**Status:** Em análise.

---

### Nível de otimização do MVP

Ainda não foi definido o nível de otimização esperado para reduzir o número de viagens.

Questões:

- O MVP utilizará uma estratégia gulosa simples?
- Será necessário testar várias combinações de pacotes?
- A solução garantirá o resultado matematicamente ótimo ou apenas uma boa solução?

**Status:** Em análise.

---

### Realização de várias viagens

Ainda não foi definido se um drone poderá executar mais de uma viagem durante a simulação.

Questões:

- Cada drone poderá realizar várias viagens?
- Existirá algum limite de viagens?
- O tempo e a disponibilidade serão considerados no MVP?

**Status:** Em análise.

---

### Pedidos impossíveis de atender

Ainda não foi definido o comportamento para pedidos que excedam o peso ou o alcance de todos os drones.

Questões:

- Os demais pedidos continuarão sendo processados?
- Os pedidos impossíveis serão listados separadamente?
- A simulação inteira deverá ser rejeitada?

**Status:** Em análise.