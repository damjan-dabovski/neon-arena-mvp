import { browser } from "$app/environment";
import { readable } from "svelte/store";
import { uniqueNamesGenerator, NumberDictionary, adjectives, animals } from 'unique-names-generator';

const LOCAL_ID_KEY = 'localIdentityKey';

const numberDictionary = NumberDictionary.generate({ min: 10, max: 99 });

const characterName: string = uniqueNamesGenerator({
    dictionaries: [adjectives, animals, numberDictionary],
    length: 3,
    separator: '',
    style: 'capital'
});

export const localIdentity = readable("", set => {
    if (!browser) {
        set("");
        return;
    }

    const idFromLocalStorage = localStorage.getItem(LOCAL_ID_KEY);

    if (idFromLocalStorage) {
        set(idFromLocalStorage)
    } else {
        localStorage.setItem(LOCAL_ID_KEY, characterName);
        set(characterName);
    }
})