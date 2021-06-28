Feature: SniperStrategyFeatures
	Basic checks of sniper strategy behaviour

Scenario: Order should be accepted
	Given it is 8:00
	When new order is received
	Then order is accepted

Scenario: Order exceeding size limits should be rejected
	Given it is 8:00
	When new order is received with params
		| Field        | Value  |
		| Symbol       | AZN.L  |
		| Side         | Buy    |
		| Price        | 100    |
		| Quantity     | 100000 |
		| StrategyType | Sniper |
	Then order is rejected

Scenario: Order exceeding price limits should be rejected
	Given it is 8:00
	When new order is received with params
		| Field        | Value  |
		| Symbol       | AZN.L  |
		| Side         | Buy    |
		| Price        | 1      |
		| Quantity     | 1000   |
		| StrategyType | Sniper |
	Then order is rejected

Scenario: Child order should be sent if favorable market data received
	Given new order is received
	And order is accepted
	When new market data is received AZN.L: 1000@8 - 200@9
	Then child order was sent

Scenario: Child order should not be sent if there is no market data
	Given it is 8:00
	When new order is received
	And order is accepted
	Then child order should not be sent

Scenario: Child order should not be sent if market data unfavorable
	Given it is 8:00
	And new order is received
	And order is accepted
	When new market data is received AZN.L: 1000@118 - 200@120
	Then child order should not be sent