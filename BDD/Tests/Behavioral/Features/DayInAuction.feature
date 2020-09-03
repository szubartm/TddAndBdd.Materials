Feature: DayInAuction

Scenario: SimpleScenario 
Given it is 8:00
When new order is received
Then order is accepted


Scenario: ScenarioWithCustomOrder and auction change
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When it is 16:31
Then child order was sent

Scenario: ScenarioWithCustomOrder, auction change and parent mod
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When it is 16:31
Then child order was sent
When child order is filled with 200 shares
And order is updated to 101 price and 800 quantity 
Then order is rejected

Scenario: ScenarioWithCustomOrder, auction change and parent mod2
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When it is 16:31
Then child order was sent
When child order is filled with 200 shares
And order is updated to 101 price and 1200 quantity 
Then order is rejected

Scenario: ScenarioWithCustomOrder, auction change and parent mod3
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When it is 16:31
Then child order was sent
When child order is filled with 200 shares
And order is updated to 90 price and 1000 quantity 
Then order is rejected

Scenario: ScenarioWithCustomOrder, auction change and parent mod4
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When it is 16:31
Then child order was sent
When order is updated to 101 price and 1000 quantity 
Then order is rejected

Scenario: ScenarioWithCustomOrder, auction change and session change once more
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When it is 16:31
Then child order was sent
When it is 16:35
Then child order is cancelled


Scenario: ScenarioWithCustomOrder and parent mod
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When order is updated to 101 price and 1000 quantity 
Then order is accepted

Scenario: ScenarioWithCustomOrder and parent mod2
Given it is 8:00
When new order is received with params
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Then order is accepted
When order is updated to 98 price and 800 quantity 
Then order is accepted

Scenario: SimpleScenario 2
Given it is 16:32
When new order is received
Then order is accepted
And child order was sent

Scenario: SimpleScenario 3
Given it is 16:32
When new order is received
Then order is accepted
And child order was sent
And Store child orders
When child order is rejected
Then child order was sent

Scenario: SimpleScenario 4
Given it is 16:32
When new order is received
Then order is accepted
And child order was sent
When order is cancelled
Then child order is cancelled