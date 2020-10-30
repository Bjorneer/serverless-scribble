
const defaultUrl = 'https://test.com/api'

export const APIUrls = {
    create: '/create',
    join: '/join',
    getUpdates: '/updates',

};

const postRequest = (url, data) => {
    return fetch(defaultUrl + url, {
        method: 'POST',
        mode: 'cors',
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
    return: {
        create: (data) => {
            return postRequest(APIUrls.create, data);
        },
        join: (data) => {
            return postRequest(APIUrls.join, data);
        },
        getUpdates: () => {
            return getRequest(APIUrls.getUpdates + '?token=' + localStorage['token']);
        },
    }
};