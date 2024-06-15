VAR currentDialogue = ""

{currentDialogue == "morningGreeting":
        Sarah : colere : Me parle pas dès le matin
        Lou : surpris : Ok ça marche
        -> END
}
{currentDialogue == "Encounter":
        Sarah : joyeux : Bonjour
        Sarah : joyeux : Oh, c'est toi la nouvelle ?
        Lou : joyeux : Salut , moi c'est Lou, et toi ?
        Sarah : joyeux : Moi c'est Sarah.
    - else:
        Sarah : joyeux : Salut
        Lou : joyeux : Salut
        -> END
}