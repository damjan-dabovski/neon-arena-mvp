<script>
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

<div class="flex flex-col m-5">
	<h1 class="text-5xl text-bold text-slate-600">Welcome to the Lobby</h1>
	<p class="text-slate-600 text-xl">Pick and join a lobby bellow</p>
	<ul class="flex flex-col gap-5 mt-10">
		{#each lobbies as { name, description, createdAt, currentPlayers, maxPlayers }}
			<li
				class="px-5 py-3 shadow-md bg-slate-50 hover:bg-slate-100 hover:cursor-pointer hover:shadow-lg w-fit rounded flex flex-row gap-4"
			>
				<div class="flex flex-col gap-1">
					<div class="flex gap-2 items-center">
						{#if currentPlayers >= maxPlayers}
							<div class="p-1 bg-red-700 rounded-full animate-ping"></div>
						{:else}
							<div class="p-1 bg-green-700 rounded-full animate-ping"></div>
						{/if}
						<div class="text-slate-700">
							{currentPlayers}/{maxPlayers}
						</div>

						<div class="text-xl text-slate-700">{name}</div>
						{#if currentPlayers >= maxPlayers}
							<div class="text-red-400">FULL</div>
						{/if}
					</div>
					<div class="text-sm text-slate-500">{createdAt.toTimeString()}</div>
					<div class="text-md text-slate-500 w-96">{description}</div>
				</div>
			</li>
		{/each}
	</ul>
</div>
