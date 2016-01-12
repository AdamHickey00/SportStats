# SportStats
Http service for serving sport stats

## Building

Initial build:
```
mono ./.paket/paket.bootstrapper.exe
```
Then,
```
mono ./.paket/paket.exe install
```

Mono build
```shell
./build.sh
```

## Running

To start the http service
```shell
./start.sh
```

## API endpoints

Lowest tournament total
```
GET http://127.0.0.1:8083/Golf/LowestTournament?firstName=Tiger&lastName=Woods
```

Lowest tournament round
```
GET http://127.0.0.1:8083/Golf/LowestRound?firstName=Tiger&lastName=Woods
```

Total tournament earnings
```
GET http://127.0.0.1:8083/Golf/TotalEarnings?firstName=Tiger&lastName=Woods
```

Sample JSON Response
```
{
  "FirstName": "Tiger",
  "LastName": "Woods",
  "Stat": {
    "Case": "LowestTournament",
    "Fields": [-27]
  }
}
```
