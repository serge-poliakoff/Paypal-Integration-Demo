let submit;
let name;
let pass;
window.addEventListener('DOMContentLoaded', ()=> {
	alert("Thats not ok");
	submit = document.getElementById("submit");
	submit.addEventListener('click', register_one());
//for
});

function register_one(){
	fetch("http://localhost:5192/Autorisation/registration", {
		mode: "no-cors",
		method: "POST",
		body: JSON.stringify({
			Id: 1,
			name: "Fix my bugs",
			pass: "oiwqf"
		}),
		headers: {
			"Content-type": "text/plain; charset=UTF-8"
		}
	})
   .then((response) => response.json())
   .then((json) => console.log(json));
}

//maction="http://localhost:5192/Autorisation/registration"