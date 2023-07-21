$(document).ready(function () {
	// error set
	function setError(error) {
		// link handler
		var errDiv = $("#errorOutput");
		// set error our component
		errDiv.text(error);
	}
	// base url
	var baseURL = "https://storageaccountnewsb9165.blob.core.windows.net/b2clogin/";
	// visual feedback element
	var visualFeedback;
	// fields
	var emailAddress = document.querySelector("#email");
	var passwordInput = document.querySelector("#newPassword");
	var passwordConfirm = document.querySelector("#reenterPassword");
	var preferredUserName = document.querySelector("#extension_PreferredUsername");
	// create an image area next to the preferred username checker
	var resIMG = document.createElement('img');
	// set attribute
	resIMG.setAttribute('id', 'visualFeedback');
	// load empty
	resIMG.setAttribute('src', baseURL + 'empty.png');
	// store element
	visualFeedback = resIMG;
	//--------------- Necessary because b2c keeps hiding the fields on something it doesn't like
	// email address 
	if (emailAddress !== null && typeof emailAddress !== "undefined" && emailAddress.length !== 0) {
		// create an observer
		var eaObserver = new MutationObserver(function (mutations) {
			// iterate over changes
			mutations.forEach(function (mutation) {
				// remove the stupid invalid key
				$("#email").removeClass('invalid');
			});
		});
		// set the observer to observe attribute changes
		eaObserver.observe(emailAddress, { attributes: true });
	}
	// password input
	if (passwordInput !== null && typeof passwordInput !== "undefined" && passwordInput.length !== 0) {
		// create an observer
		var piObserver = new MutationObserver(function (mutations) {
			// iterate over changes
			mutations.forEach(function (mutation) {
				// remove the stupid invalid key
				$("#newPassword").removeClass('invalid');
			});
		});
		// set the observer to observe attribute changes
		piObserver.observe(passwordInput, { attributes: true });
	}
	// password confirm 
	if (passwordConfirm !== null && typeof passwordConfirm !== "undefined" && passwordConfirm.length !== 0) {
		// create an observer
		var pcObserver = new MutationObserver(function (mutations) {
			// iterate over changes
			mutations.forEach(function (mutation) {
				// remove the stupid invalid key
				$("#reenterPassword").removeClass('invalid');
			});
		});
		// set the observer to observe attribute changes
		pcObserver.observe(passwordConfirm, { attributes: true });
	}
	// preferred user field section
	if (preferredUserName !== null && typeof preferredUserName !== "undefined" && preferredUserName.length !== 0) {
		// create an observer
		var puObserver = new MutationObserver(function (mutations) {
			// iterate over changes
			mutations.forEach(function (mutation) {
				// remove the stupid invalid key
				$("#extension_PreferredUsername").removeClass('invalid');
			});
		});
		// set the observer to observe attribute changes
		puObserver.observe(preferredUserName, { attributes: true });
		// create div element for our errors
		var divEH = document.createElement('div');
		// set the id 
		divEH.setAttribute('id', 'errorOutput');
		// create div element for the visual feedback
		var divVF = document.createElement('div');
		// set the id
		divVF.setAttribute('id', 'visualFeedbackContainer');
		// append the image
		divVF.appendChild(visualFeedback);
		// adjust the padding of the text attribute
		$("#extension_PreferredUsername").css('width', '144px');
		// insert the div field above the textbox
		preferredUserName.insertAdjacentElement('beforebegin', divEH);
		// insert the div field right after the textbox
		preferredUserName.insertAdjacentElement('afterend', divVF);
	}
	// hide the preferred username field on load
	$("#attributeList > ul > li:nth-child(7) > div").hide();
	// get how the username is to be selected
	$("input[name='extension_UsernameSelectionType']").on("change", function () {
		// value choice
		var selectionType = $(this).val();
		// if the choice is random
		if (selectionType == "Random") {
			$("#attributeList > ul > li:nth-child(7) > div").hide();
		} else {
			$("#attributeList > ul > li:nth-child(7) > div").show();
		}
	});
	// on changes to the preferred user name field
	$("#extension_PreferredUsername").on("input", function (event) {
		// get the input from the preferred user name field
		var preferredUserName = $(this).val();
		var fieldLength = preferredUserName.length; event.stopPropagation();
		// store the error text
		var errTextOut = "";
		var isSetError = false;
		// reset the values of the error out
		setError("");
		// if length is zero
		if (fieldLength == 0) {
			// set the visual feedback to nothing
			$('#visualFeedback').attr('src', baseURL + 'empty.png');
		} else {
			// regex 
			var re = /^(?!.*__)(?!.*_$)(?!.*\.\.)(?!.*\.$)[A-Za-z](\w|[\.])*$/;
			// validation check 
			// length test
			if (fieldLength > 64 || fieldLength < 4) {
				errTextOut = "Usernames must be between 4 and 64 characters in length.";
				isSetError = true;
			}
			// regex test
			if (!re.test(preferredUserName)) {
				errTextOut = "Usernames must start with a letter, may include numbers and special characters \"._\", and special characters cannot be doubled or be the last character.";
				isSetError = true;
			}
			// check error state
			if (isSetError) {
				// set an error
				setError(errTextOut);
			} else {
				// build a url
				var url = "https://api.newsboard.email/member/check/" + encodeURIComponent(preferredUserName);
				// ajax call
				$.ajax({
					xhrFields: { withCredentials: true },
					type: "GET",
					url: url
				}).done(function (data) {
					// check return state
					if (typeof data.success !== "undefined") {
						// if we got a response
						if (data.success === true) {
							// set the good symbol
							$('#visualFeedback').attr('src', baseURL + 'greencheck.png');
						} else {
							// set an error
							setError("That username is unavailable.");
							// set the error symbol
							$('#visualFeedback').attr('src', baseURL + 'redx.png');
						}
					} else {
						// set an error
						setError("We apologize but an error occurred.");
						// set the error symbol
						$('#visualFeedback').attr('src', baseURL + 'redx.png');
					}
				});
			}
		}
	});
});	