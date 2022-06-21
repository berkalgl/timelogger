const BASE_URL = 'http://localhost:3001/api';

export async function Get(sourceUrl, successCallBack, failedCallback) {
	await fetch(BASE_URL + sourceUrl)
	.then(results => results.json())
	.then(data => {
		successCallBack(data.result);
	})
	.catch((error) => {
		failedCallback(error)
	});
}

export async function Post(sourceUrl, input, successCallBack, failedCallback) {
	
	const requestOptions = {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify(input)
	};
	await fetch(BASE_URL + sourceUrl, requestOptions)
		.then(response => response.json())
		.then(data => {
			successCallBack(data)
		})
		.catch((error) => {
			failedCallback(error)
		});
}
export async function Put(sourceUrl, successCallBack, failedCallback) {
	const requestOptions = {
		method: 'PUT',
		headers: { 'Content-Type': 'application/json' }
	};
	await fetch(BASE_URL + sourceUrl,requestOptions)
	.then(results => results.json())
	.then(data => {
		successCallBack(data);
	})
	.catch((error) => {
		failedCallback(error)
	});
}