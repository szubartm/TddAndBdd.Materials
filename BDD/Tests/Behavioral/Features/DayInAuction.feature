Feature: DayInAuction

Scenario: In the middle of the OpenAuction time period order should be accepted
Given it is OpenAuction/Beginning
When new order is received
Then order is accepted

Scenario: In the Beginning of the IntradayAuction time period order should be accepted
Given it is IntradayAuction/Beginning
When new order is received
Then order is accepted

Scenario: Order exceeding size limits should be rejected
	Given it is OpenAuction/End
	When new order is received with params
		| Field        | Value  |
		| Symbol       | AZN.L  |
		| Side         | Buy    |
		| Price        | 100    |
        | Quantity     | 100000 |
		| StrategyType | Sniper |
	Then order is rejected

Scenario: Order exceeding price limits should be rejected
	Given it is OpenAuction/Beginning
	When new order is received with params
		| Field        | Value  |
		| Symbol       | AZN.L  |
		| Side         | Buy    |
		| Price        | 1      |
		| Quantity     | 1000   |
		| StrategyType | Sniper |
	Then order is rejected