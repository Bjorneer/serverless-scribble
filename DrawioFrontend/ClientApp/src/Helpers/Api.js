
const defaultUrl = 'http://localhost:7071/api'

export const APIUrls = {
    create: '/CreateGame',
    join: '/JoinGame',
    getUpdates: '/GetGameState',

};

const postRequest = (url, data) => {
    return fetch(defaultUrl + url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
};

const getRequest = (url) => {
    return fetch(defaultUrl + url);
};

export const GameAPI = {
    create: (data) => {
        return postRequest(APIUrls.create, data);
    },
    join: (data) => {
        return postRequest(APIUrls.join, data);
    },
    getUpdates: () => {
        return getRequest(APIUrls.getUpdates + '?token=' + localStorage['token']);
    },
};