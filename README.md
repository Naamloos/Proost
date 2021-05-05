# FeestSpel
Een open-source party drinking game.. Drink met mate(n)!

# Template voor een pack
```JSON
{
  "Title": "Template",
  "Description": "Template voor een pack",
  "Author": "naam",
  "AuthorUrl": "link.nl/naam",
  "Missions": [
    {
      "MissionText": "{0} en {1} spelen steen-papier-schaar.",
      "SubjectCount": 2,
      "TakesDrinks": "De verliezer neemt {0} slokken.",
      "FinishesGlass": "De verliezer drinkt zijn glas leeg."
    },
    {
      "MissionText": "Iedereen die een zwart shirt draagt",
      "SubjectCount": 0,
      "TakesDrinks": "neemt {0} slokken.",
      "FinishesGlass": "drinkt zijn glas leeg."
    }
  ],
  "SubMissions": 
  [
    {
      "Activation": "{0} en {1} zijn nu drinkmaatjes! Als een van de twee een slok neemt, dan moet de ander ook een slok nemen.",
      "Deactivation": "{0} en {1} zijn geen drinkmaatjes meer.",
      "SubjectCount": 1
    },
    {
      "Activation": "Bijnaam! {0} mag een bijnaam bedenken voor {1}. Als iemand {1} niet bij deze bijnaam noemt, moet hij of zij drinken.",
      "Deactivation": "{1} heeft geen bijnaam meer. Thanks, {0}!",
      "SubjectCount": 1
    }
  ]
}
```