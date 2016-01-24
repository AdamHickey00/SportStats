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
    if (request.readyState == 4 && request.status == 200) {
      var response = JSON.parse(request.responseText);
      var statValue = response.Stat.Fields[0];
      document.getElementById(endpoint).innerHTML = statValue;
    }
  };

  request.open("GET", url, true);
  request.send();
}
