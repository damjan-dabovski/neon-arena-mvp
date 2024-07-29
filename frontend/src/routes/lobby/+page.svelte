<script>
	import { signalRConnection } from '$lib/stores/signalRStore';

	signalRConnection.subscribe((v) => {
		v.on('/something', (data) => console.log(data));
		v.on('/something-else', (data) => console.log(data));
		v.on('/error', (error) => console.error(error));
	});

	const lobbies = [
		{
			name: 'Some Lobby',
			description:
				'Voluptate nisi magna ipsum ullamco do aliqua minim reprehenderit eiusmod id eu.',
			createdAt: new Date(),
			currentPlayers: 2,
			maxPlayers: 10
		},
		{
			name: 'Some Lobby',
			description:
				'Voluptate nisi magna ipsum ullamco do aliqua minim reprehenderit eiusmod id eu.',
			createdAt: new Date(),
			currentPlayers: 3,
			maxPlayers: 10
		},
		{
			name: 'Some Lobby',
			description:
				'Voluptate nisi magna ipsum ullamco do aliqua minim reprehenderit eiusmod id eu.',
			createdAt: new Date(),
			currentPlayers: 5,
			maxPlayers: 10
		},
		{
			name: 'Some Lobby',
			description:
				'Voluptate nisi magna ipsum ullamco do aliqua minim reprehenderit eiusmod id eu.',
			createdAt: new Date(),
			currentPlayers: 10,
			maxPlayers: 10
		},
		{
			name: 'Some Lobby',
			description:
				'Voluptate nisi magna ipsum ullamco do aliqua minim reprehenderit eiusmod id eu.',
			createdAt: new Date(),
			currentPlayers: 1,
			maxPlayers: 10
		}
	];
</script>

<div class="flex flex-col flex-grow p-5 bg-slate-800">
	<ul class="flex flex-col gap-5 mt-10 w-full items-center">
		{#each lobbies as { name, description, createdAt, currentPlayers, maxPlayers }}
			<li
				class="px-5 py-3 transition-all hover:scale-[102%] hover:cursor-pointer w-full md:w-1/2 xl:w-1/3 rounded flex flex-row gap-4 bg-slate-100 backdrop-blur-md bg-opacity-75"
			>
				<div class="flex flex-col gap-1">
					<div class="flex gap-2 items-center">
						{#if currentPlayers >= maxPlayers}
							<div class="p-1 bg-red-700 rounded-full animate-ping"></div>
						{:else}
							<div class="p-1 bg-green-700 rounded-full animate-ping"></div>
						{/if}
						<div class="text-slate-900">
							{currentPlayers}/{maxPlayers}
						</div>

						<div class="text-xl text-slate-900">{name}</div>
						{#if currentPlayers >= maxPlayers}
							<div class="text-red-400">FULL</div>
						{/if}
					</div>
					<div class="text-sm text-slate-700">{createdAt.toTimeString()}</div>
					<div class="text-md text-slate-700">{description}</div>
				</div>
			</li>
		{/each}
	</ul>
</div>
