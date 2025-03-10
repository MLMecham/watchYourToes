#CHATBOT STUFF

from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

class Character(BaseModel):
    name: str
    level: int
    

@app.post("/level-up")
async def level_up(character: Character):
    print("Received character:", character.dict())  # Debugging output
    character.level += 1  # Increase level
    print("Returned character:", character.dict())  # Debugging output
    return character

