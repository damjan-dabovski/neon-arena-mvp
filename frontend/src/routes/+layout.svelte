<script>
	import { browser } from '$app/environment';
	import { localIdentity } from '$lib/stores/localIdentityStore';
	import '../app.css';

	let identity = '';

	localIdentity.subscribe((id) => (identity = id));
</script>

<div class="flex flex-col h-screen">
	<header class="w-full fixed top-0 z-50 shadow-md">
		<div
			class="h-12 px-6 w-full bg-slate-900 text-slate-100 text-end flex items-center gap-1 justify-between"
		>
			<h1 class="font-mono font-bold text-3xl">
				<a class="block hover:scale-[102%] active:scale-[99%] hover:bg-slate-800 rounded-sm px-3 -ml-3 transition-all" href="/lobby">neon_arena</a>
			</h1>
			<div>
				{#if browser}
					Playing as <span class="font-mono font-bold underline">{identity}</span>
				{/if}
			</div>
		</div>
	</header>
	<div class="h-12"></div>
	<slot />
</div>
