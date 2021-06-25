# 1. Definitions

Parent order – order received from outside of the Trading System.

Child order – order created by Trading System based on parent order.

# 2. System

## 1. Components

### 1.1. Data Provider System

Provides market data to Trading System.

- InsertNewMarketData / ClearMarketData

### 1.2. Order Management System

- Insert / Acknowledge/ Cancel / Fill / Reject / Update

- GetAll / GetAllChilds / GetById

### 1.3. Trading System

- Time Simulator - SetTimeTo / UpdateTimeBy

- Strategies

## 2. Strategies

### 2.1. Day

#### Constraints

| Min Parent Order Size | 100 |
| Max Parent Order Size | 90000 |
| Min Parent Order Price | Not specified |
| Max Parent Order Price | 1000 |

#### Description

1. Strategy is active only during OpenAuction and IntradayAuction Trading Sessions.
2. When new parent order arrives it&#39;s validated against constraints.
	- If it does not meet them, the parent order should be rejected.
	- In other cases parent order should be acknowledged.
3. Exchange in OpenAuction or IntradayAuction
	- Trading Session changed to anything other than these 2 sessions
		1. Child order should be cancelled
		2. Strategy starts to behave like in point 4.
	- Parent order modifications are rejected.
	- In case of receiving parent order cancellation:
		1. Child order should be cancelled
		2. Parent order cancellation should be accepted
		3. Strategy finishes its work.
	- In case of receiving child order reject, new child order should be sent.
	- Child order partially filled =&gt; update state, wait until fully filled.
		1. Child order fully filled =&gt; Strategy finishes its work.
4. Exchange in other Trading Sessions
	  - Strategy waits for session change to one of the point 3.
	  - Parent order modifications are accepted as long as they meet the strategy constraints. Otherwise rejected.
	  - Parent order cancellations are accepted.
	  - In case of Trading Session change to OpenAuction or IntradayAuction child order should be sent matching price &amp; size of parent.

### 2.2. Sniper

#### Constraints

| Min Parent Order Size | 100 |
| Max Parent Order Size | 90000 |
| Min Parent Order Price | 10 |
| Max Parent Order Price | 1000 |

#### Description

1. Strategy does not pay attention to the trading sessions.
2. When new parent order arrives it&#39;s validated against constraints.
	  - If it does not meet them, the parent order should be rejected. Strategy finishes its work. (=Should not react to any market data or trading session changes).
	  - In other cases parent order should be acknowledged. Strategy waits for new market data related to parent order symbol.
3. When waiting for market data
	  - Parent order cancellations are accepted. Strategy finishes its work.
	  - Parent order modifications are accepted as long as they meet the strategy constraints. Otherwise rejected.
	  - When new market data becomes available
			1. If they are favorable (e.g. for a buy order, if the order price is not lower than the sell offer) new child order with TimeInForce = FOK should be sent. Otherwise no reaction.
4. When child order was sent
	  - Received child order rejection =&gt; system behaves as if no child order was sent and goes to Waiting for market data state.
	  - Child order cancellation =&gt; system behaves as if no child order was sent and goes to Waiting for market data state.
	  - Child order fully filled on market =&gt; parent order is fully filled. Strategy finishes its work.
	  - Child order partially filled =&gt; updates child order internal state, waits for other actions.
	  - Parent order cancellations are accepted only if system was able to cancel child order. Strategy finishes its work. Otherwise parent order cancellations are rejected.
	  - Parent order modifications are accepted only in case of quantity increase without price change, price change to &quot;less restrictive&quot; (for buy orders increasing price, for sell orders decreasing) without quantity change or when quantity is also increased. Child order should be updated to new values. Otherwise modification should be rejected.

# 3. Enumerations, constants and configuration values

## 1. Enumerations

### 1.1. Side

Buy = 0,

Sell = 1

### 1.2. TimeInForce

Day = 1,

IOC = 2,

FOK = 3

### 1.3. State

UNACK = 0,

LIVE = 1,

PMOD = 2,

PFILLED = 3,

FILLED = 4,

PCANCEL = 5,

CANCELLED = 6,

REJECTED = 7,

CLOSED = 8

### 1.4. StrategyTypes

Day = 0,

Sniper = 1

## 2. Constants and configuration values

| Trading Session | Hours |
| --- | --- |
| MarketClosed | 00:00:00 - 7:00:00, 16:35:00 - 24:00:00 |
| OpenAuction | 7:00:00 - 8:00:00 |
| ContinuesTrading | 8:00:00 - 12:00:00, 12:05:00 - 16:30:00 |
| IntradayAuction | 12:00:00 - 12:05:00,16:30:00 - 16:35:00 |