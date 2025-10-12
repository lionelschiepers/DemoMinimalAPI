import http from 'k6/http'
import { check, sleep } from 'k6'

export const options = {
    vus: 10,
    duration: '30s',
};

const url = 'http://localhost/';


export default function () {
    const data = {
        "var": "val"
    }

    let res = http.post(url, JSON.stringify(data), {
        headers: { 'Content-Type': 'application/json' },
    });

    check(res, { 'success login': (r) => r.status === 200 })
}
