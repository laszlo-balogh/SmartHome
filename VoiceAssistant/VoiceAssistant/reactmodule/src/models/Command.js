import {v4 as uuidV4} from "uuid"
class Command{
    constructor(commandSentense){
        this.id = uuidV4()
        this.commandSentense=commandSentense
        this.source = "react"
    }
}

export default Command