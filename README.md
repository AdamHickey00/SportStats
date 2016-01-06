# SportStats
Http service for serving sport stats

## Building

Mono build
```shell
./build.sh
```

## Running

To start the http service
```shell
./start.sh
```

## Sample Endpoint Call

Hole in ones
```
GET http://127.0.0.1:8083/Golf/LowestTournament?firstName=Tiger&lastName=Woods
```

JSON Response
```
{
  "FirstName": "Tiger",
  "LastName": "Woods",
  "Accomplishment": {
    "Case": "HoleInOnes",
    "Fields": [7]
  }
}
```
