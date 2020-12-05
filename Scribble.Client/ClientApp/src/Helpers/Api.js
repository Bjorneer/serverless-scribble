
export const baseUrl = process.env.REACT_APP_API_BASE_URL;

export const APIUrls = {
    create: '/create',
    join: '/join',
    guess: '/guess',
    draw: '/draw',
    joinGroup: '/joinGroup',
    leaveGroup: '/leaveGroup',
    negotiate: '/negotiate',
    start: '/start'
};

const postRequest = (url, data, options) => {
    return fetch(baseUrl + url, {
        method: 'POST',
        mode: 'cors',
        body: JSON.stringify(data),
        ...options
    });
};

export const ApiFactory = {
    create: (data) => {
        return postRequest(APIUrls.create, data);
    },
    join: (data) => {
        return postRequest(APIUrls.join, data);
    },
    start: (data) => {
        return postRequest(APIUrls.start, data);
    },
    guess: (data) => {
        return postRequest(APIUrls.guess, data);
    },
    draw: (data) => {
        return postRequest(APIUrls.draw, data);
    },
    joinGroup: (data, options) => {
        return postRequest(APIUrls.joinGroup, data, options);
    },
    leaveGroup: (data, options) => {
        return postRequest(APIUrls.leaveGroup, data, options);
    },
    negotiate: (data, options) => {
        return postRequest(APIUrls.negotiate, data, options)
    }
};