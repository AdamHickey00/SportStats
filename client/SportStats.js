function getStat(sport, endpoint) {
  var request = new XMLHttpRequest();
  var firstName = document.getElementById('golfFirst').value;
  var lastName = document.getElementById('golfLast').value;;

  if (sport === 'Baseball') {
    firstName = document.getElementById('baseballFirst').value;;
    lastName = document.getElementById('baseballLast').value;;
  }

  var url = "http://127.0.0.1:8083/" + sport + "/" + endpoint + "?firstName=" + firstName + "&lastName=" + lastName;

  request.onreadystatechange = function() {
    if (request.readyState == 4) {
      if (request.status == 200) {
        var response = JSON.parse(request.responseText);
        document.getElementById(endpoint).innerHTML = response.Stat.Fields[0];
      }
      else {
        document.getElementById(endpoint).innerHTML = request.responseText;
      }
    }
  };

  request.open("GET", url, true);
  request.send();
}

function getGolfStats() {
  getStat('Golf', 'LowestTournament');
  getStat('Golf', 'LowestRound');
  getStat('Golf', 'TotalEarnings');
}

function getBaseballStats() {
  getStat('Baseball', 'Homeruns');
  getStat('Baseball', 'Strikeouts');
  getStat('Baseball', 'Steals');
}
