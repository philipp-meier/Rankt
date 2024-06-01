#!/usr/bin/python3
import requests
import string
import random
import time

def generate_username():
    characters = string.ascii_letters + string.digits
    username = ''.join(random.choice(characters) for _ in range(random.randint(5, 32)))
    return username
    
for i in range(1500):
    username = generate_username()
    
    req_start = time.time()
    x = requests.post('https://localhost:7151/api/login', json={'username': username, 'password': 'temp'}, verify=False)
    req_end = time.time()
    
    print(f"Request {i+1}: Username \"{username}\", StatusCode: {x.status_code} (duration: {req_end-req_start} seconds)")